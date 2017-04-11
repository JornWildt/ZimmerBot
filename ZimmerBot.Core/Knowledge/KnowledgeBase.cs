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

      Topics[DefaultTopicName] = new Topic(DefaultTopicName);

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


    public StandardRule AddRule(string label, string topicName, WRegexBase pattern, List<RuleModifier> modifiers, List<Statement> statements)
    {
      Logger.DebugFormat("Found rule: {0}", pattern);

      topicName = topicName ?? DefaultTopicName;
      Topic topic = Topics[topicName];

      StandardRule r = new StandardRule(this, label, topic, pattern, modifiers, statements);
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


#if false
    public void FindCurrentTopic(TriggerEvaluationContext context)
    {
      int bestWordMatchCount = 1;
      List<Topic> bestTopics = new List<Topic>();

      if (context.InputContext.Input != null)
      {
        foreach (Topic t in Topics.Values)
        {
          int matchCount = 0;
          foreach (string word in t.TriggerWords)
          {
            foreach (var token in context.InputContext.Input)
            {
              if (word.Equals(token.OriginalText, StringComparison.CurrentCultureIgnoreCase))
                ++matchCount;
            }
          }

          if (matchCount == bestWordMatchCount)
          {
            bestTopics.Add(t);
          }
          else if (matchCount > bestWordMatchCount)
          {
            bestWordMatchCount = matchCount;
            bestTopics.Clear();
            bestTopics.Add(t);
          }
        }

        string currentTopicName = context.InputContext.Session.CurrentTopic();

        string selectedTopicName = bestTopics.Count > 0
          ? bestTopics[Randomizer.Next(bestTopics.Count)].Name
          : (currentTopicName ?? DefaultTopicName);

        context.InputContext.Session.SetCurrentTopic(selectedTopicName);
      }
    }
#endif


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

        // Does user input match anything in current topic?
        foreach (StandardRule r in topic.StandardRules)
        {
          IList<Reaction> result = r.CalculateReactions(context);
          foreach (Reaction reaction in result)
            reactions.Add(reaction);
        }

        // No match in current topic - use topic story reaction
        int topicRuleIndex = context.InputContext.Session.GetTopicRuleIndex(topicName);
        if (reactions.Count == 0 && topic.TopicRules.Count > topicRuleIndex)
        {
          foreach (Reaction reaction in topic.TopicRules[topicRuleIndex].CalculateReactions(context))
            reactions.Add(reaction);

          context.InputContext.Session.SetTopicRuleIndex(topicName, topicRuleIndex + 1);

          // No topic stories left => clear topic
          if (topicRuleIndex == topic.TopicRules.Count-1)
            context.InputContext.Session.SetCurrentTopic(null);
        }

        // FIXME: look at default topic
      }

      return reactions;
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
