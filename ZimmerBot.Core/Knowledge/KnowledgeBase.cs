using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CuttingEdge.Conditions;
using log4net;
using Quartz;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline;
using ZimmerBot.Core.Pipeline.InputStages;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.StandardProcessors;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class KnowledgeBase
  {
    static ILog Logger = LogManager.GetLogger(typeof(KnowledgeBase));

    public static Random Randomizer = new Random();

    const string DefaultTopicName = "#DefaultTopic";


    public enum InitializationMode { Clear, Restore, RestoreIfExists }

    public RDFStore MemoryStore { get; protected set; }

    public IDictionary<string, Concept> Concepts { get; protected set; }

    public IDictionary<string, Entity> Entities { get; protected set; }

    public IDictionary<string,Topic> Topics { get; protected set; }

    public Topic DefaultTopic { get { return Topics[DefaultTopicName]; } }

    public IEnumerable<StandardRule> DefaultRules { get { return Topics[DefaultTopicName].StandardRules; } }

    public IEnumerable<StandardRule> AllRules { get { return Topics.Values.SelectMany(t => t.StandardRules); } }

    public IDictionary<Request.EventEnum, List<StandardRule>> EventHandlers { get; protected set; }

    public Pipeline<InputPipelineItem> InputPipeline { get; protected set; }

    protected IDictionary<string, Rule> LabelToRuleMap { get; set; }

    protected IList<string> SparqlForEntities { get; set; }

    public KnowledgeBase()
      : this("default")
    {
    }


    public KnowledgeBase(string memoryId)
    {
      MemoryStore = new RDFStore(memoryId);
      Concepts = new Dictionary<string, Concept>();
      Topics = new Dictionary<string, Topic>();
      Entities = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
      EventHandlers = new Dictionary<Request.EventEnum, List<StandardRule>>();
      LabelToRuleMap = new Dictionary<string, Rule>();
      SparqlForEntities = new List<string>();

      Topics[DefaultTopicName] = new Topic(DefaultTopicName, isAutomaticallySelectable: false);

      InputPipeline = new Pipeline<InputPipelineItem>();
      InputPipeline.AddHandler(new WordTaggingStage());
      InputPipeline.AddHandler(new EntityTaggingStage());
      InputPipeline.AddHandler(new TopicAssigningStage());
      InputPipeline.AddHandler(new ReactionGeneratorStage());
      InputPipeline.AddHandler(new OutputGeneratorStage());
      InputPipeline.AddHandler(new ChatLoggerStage());
    }


    public void Initialize(InitializationMode mode)
    {
      MemoryStore.Initialize(mode);
    }


    public void Run()
    {
      foreach (string s in SparqlForEntities)
      {
        Logger.DebugFormat("Run query for entities: {0}", s);
        Dictionary<string, object> matches = new Dictionary<string, object>();
        IList<object> parameters = new List<object>();
        RDFResultSet output = MemoryStore.Query(s, matches, parameters);
        Logger.DebugFormat("Found {0} records", output.Count);
        foreach (var o in output)
        {
          if (o.ContainsKey("label"))
            AddEntity(o["label"]);
        }
      }
    }


    public Concept AddConcept(string name, List<List<string>> patterns)
    {
      Concept w = new Concept(this, name, patterns);
      Concepts.Add(name, w);
      return w;
    }


    public Topic AddTopic(string name)
    {
      Topic t = new Topic(name);
      Topics.Add(name, t);
      return t;
    }


    public Entity AddEntity(string label)
    {
      Entity e = new Entity(label);
      Entities[e.Label] = e;
      return e;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      foreach (StandardRule r in AllRules)
      {
        r.RegisterScheduledJobs(scheduler, botId);
      }
    }


    public StandardRule AddRule(string label, string topicName, List<WRegexBase> patterns, List<RuleModifier> modifiers, List<Statement> statements)
    {
      Logger.DebugFormat("Found rule: {0}", patterns.Select(t => t?.ToString()).Aggregate((a,b) => ">" + a + " >" + b));

      topicName = topicName ?? DefaultTopicName;
      Topic topic = Topics[topicName];

      StandardRule r = new StandardRule(this, label, topic, patterns, modifiers, statements);
      topic.AddRule(r);

      return r;
    }


    public TopicRule AddTopicRule(string label, string topicName, List<Statement> statements)
    {
      topicName = topicName ?? DefaultTopicName;
      Logger.DebugFormat("Found a topic rule for '{0}'", topicName);

      Topic topic = Topics[topicName];

      TopicRule r = new TopicRule(this, label, topic, statements);
      topic.AddRule(r);

      return r;
    }


    internal void RegisterRuleLabel(string label, Rule r)
    {
      LabelToRuleMap[label] = r;
    }


    public void RegisterEventHandler(string e, List<Statement> statements)
    {
      Request.EventEnum etype;
      if (Enum.TryParse(e, true, out etype))
      {
        StandardRule rule = new StandardRule(this, null, null, null, null, statements);
        if (!EventHandlers.ContainsKey(etype))
          EventHandlers[etype] = new List<StandardRule>();
        EventHandlers[etype].Add(rule);
      }
      else
        throw new ApplicationException($"Unknown event '{e}'");
    }


    public void RegisterSparqlForEntities(string sparql)
    {
      SparqlForEntities.Add(sparql);
    }


    public Rule GetRuleFromLabel(string label)
    {
      return LabelToRuleMap[label];
    }


    public ReactionSet FindMatchingReactions(TriggerEvaluationContext context, ReactionSet reactions = null)
    {
      if (reactions == null)
        reactions = new ReactionSet();

      if (context.InputContext.Request.EventType != null)
      {
        if (EventHandlers.ContainsKey(context.InputContext.Request.EventType.Value))
        {
          foreach (StandardRule rule in EventHandlers[context.InputContext.Request.EventType.Value])
          {
            ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(1));
            Reaction reaction = new Reaction(rc, rule, null);
            reactions.Add(reaction);
          }
        }
      }
      else
      {
        string topicName = context.InputContext.Session.CurrentTopic() ?? DefaultTopicName;
        Topic topic = Topics[topicName];

        BotUtility.EvaluationLogger.Debug($"Matching: {context.InputContext.Request.Input}");

        // Does user input match anything in current topic?
        SelectReactionsFromTopic(topic, reactions, context, 2.0);

        // No match in current topic - use topic story reaction
        int topicRuleIndex = context.InputContext.Session.GetTopicRuleIndex(topicName);
        if (reactions.Count == 0 && topic.TopicRules.Count > topicRuleIndex)
        {
          foreach (Reaction reaction in topic.TopicRules[topicRuleIndex].CalculateReactions(context, 2.0))
            reactions.Add(reaction);

          context.InputContext.Session.SetTopicRuleIndex(topicName, topicRuleIndex + 1);

          // No topic stories left => clear topic
          if (topicRuleIndex == topic.TopicRules.Count-1)
            context.InputContext.Session.SetCurrentTopic(null);
        }

        // Try all other topics than the current with a smaller weight applied
        Topic nextTopic = null;

        foreach (Topic t in Topics.Where(tp => tp.Key != topicName).Select(tp => tp.Value))
        {
          if (SelectReactionsFromTopic(t, reactions, context, 1.0))
            nextTopic = t;
        }

        if (nextTopic != null && nextTopic.IsAutomaticallySelectable)
          context.InputContext.Session.SetCurrentTopic(nextTopic.Name);
      }

      return reactions;
    }


    public bool SelectReactionsFromTopic(Topic topic, ReactionSet reactions, TriggerEvaluationContext context, double weight)
    {
      BotUtility.EvaluationLogger.Debug($"Select reactions from topic {topic.Name} with weight {weight}");

      bool reactionsAdded = false;
      foreach (StandardRule r in topic.StandardRules)
      {
        IList<Reaction> result = r.CalculateReactions(context, weight);
        foreach (Reaction reaction in result)
        {
          BotUtility.EvaluationLogger.Debug($"Got '{r.ToString()}' with score {reaction.Score}");
          if (reactions.Add(reaction))
            reactionsAdded = true;
        }
      }

      return reactionsAdded;
    }


    public void LoadFromFiles(string path, string pattern = "*.zbot", SearchOption option = SearchOption.TopDirectoryOnly)
    {
      ConfigurationParser cfg = new ConfigurationParser();

      Logger.Debug($"Scanning for '{pattern}' files in '{path}'");
      foreach (string filename in Directory.EnumerateFiles(path, pattern, option))
      {
        Logger.InfoFormat("Loading zbot file: {0}", filename);
        cfg.ParseConfigurationFromFile(this, filename);
      }
    }
  }
}
