using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Quartz;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Patterns;
using ZimmerBot.Core.Pipeline;
using ZimmerBot.Core.Pipeline.InputStages;
using ZimmerBot.Core.Scheduler;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;
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

    public IDictionary<string,Topic> Topics { get; protected set; }

    public Topic DefaultTopic { get { return Topics[DefaultTopicName]; } }

    public IEnumerable<StandardRule> DefaultRules { get { return Topics[DefaultTopicName].StandardRules; } }

    public IEnumerable<StandardRule> AllRules { get { return Topics.Values.SelectMany(t => t.StandardRules); } }

    public IDictionary<Request.EventEnum, List<StandardRule>> EventHandlers { get; protected set; }

    public IDictionary<string,ScheduledAction> ScheduledActions { get; protected set; }

    public Pipeline<InputPipelineItem> InputPipeline { get; protected set; }

    protected PipelineActionStage StartActionStage { get; set; }

    protected IDictionary<string, Rule> LabelToRuleMap { get; set; }

    public EntityManager EntityManager { get; protected set; }

    protected WordDefinitionManager WordDefinitionManager { get; set; }

    protected IList<string> SparqlForEntities { get; set; }

    public PatternManager PatternManager { get; protected set; }

    public KnowledgeBase()
      : this("default")
    {
    }


    public KnowledgeBase(string memoryId)
    {
      MemoryStore = new RDFStore(memoryId);
      Concepts = new Dictionary<string, Concept>();
      Topics = new Dictionary<string, Topic>();
      EventHandlers = new Dictionary<Request.EventEnum, List<StandardRule>>();
      LabelToRuleMap = new Dictionary<string, Rule>();
      EntityManager = new EntityManager(this);
      WordDefinitionManager = new WordDefinitionManager(this);
      SparqlForEntities = new List<string>();
      PatternManager = new PatternManager(this);
      ScheduledActions = new Dictionary<string, ScheduledAction>();

      Topics[DefaultTopicName] = new Topic(DefaultTopicName, isAutomaticallySelectable: false);

      StartActionStage = new PipelineActionStage();

      InputPipeline = new Pipeline<InputPipelineItem>();
      InputPipeline.AddHandler(StartActionStage);
      InputPipeline.AddHandler(new SpellCheckerStage());
      InputPipeline.AddHandler(new WordStemmerStage());
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
      SessionManager.Initialize(mode);
    }


    public void SetupComplete()
    {
      foreach (string s in SparqlForEntities)
      {
        Logger.DebugFormat("Run query for entities: {0}", s);
        Dictionary<string, object> matches = new Dictionary<string, object>();
        IList<object> parameters = new List<object>();
        RDFResultSet result = MemoryStore.Query(s, matches, parameters);
        Logger.DebugFormat("Found {0} records", result.Count);

        EntityManager.RegisterEntityClass("default", 
          result.Where(item => item.ContainsKey("label")).Select(item => item["label"]).ToList());
      }

      WordDefinitionManager.SetupComplete(MemoryStore);

      foreach (Rule r in AllRules)
        r.SetupComplete();

      EntityManager.SetupComplete();
      PatternManager.SetupComplete();      
    }


    public Concept AddConcept(string name, List<List<string>> patterns)
    {
      if (Concepts.ContainsKey(name))
        throw new InvalidOperationException($"Repeated definition of concept '{name}'");
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


    public void RegisterEntityClass(string className, List<WRegexBase> entityPatterns)
    {
      EntityManager.RegisterEntityClass(className, entityPatterns);
    }


    public void RegisterPatternSet(List<KeyValuePair<string, List<string>>> identifiers, List<Pattern> patterns)
    {
      PatternSet set = new PatternSet(identifiers, patterns);
      PatternManager.AddPatternSet(set);
    }


    public void RegisterDefinitions(List<string> mainClasses, List<WordDefinition> definitions)
    {
      WordDefinitionManager.RegisterDefinitions(mainClasses, definitions);
      foreach (WordDefinition wd in definitions)
      {
        if (wd.Word != null)
          EntityManager.RegisterEntity(wd.Word, wd.Alternatives, mainClasses);
      }
    }


    public void RegisterScheduledAction(string cronExpr, List<RuleModifier> modifiers, List<Statement> statements)
    {
      var action = new ScheduledAction(this, cronExpr, modifiers, statements);
      ScheduledActions.Add(action.Id, action);
    }


    public ScheduledAction GetScheduledAction(string id)
    {
      return ScheduledActions[id];
    }


    public void RegisterPipelineItem(string stage, List<RuleModifier> modifiers, List<Statement> statements)
    {
      if (stage == "start")
        StartActionStage.RegisterPipelineAction(new PipelineAction(this, modifiers, statements));
      else
        throw new ApplicationException($"Unknown pipeline stage '{stage}'.");
    }


    public StandardRule AddRegexRule(string label, string topicName, List<WRegexBase> patterns, List<RuleModifier> modifiers, List<Statement> statements)
    {
      return AddRule(topicName, topic => new StandardRule(this, label, topic, patterns, modifiers, statements));
    }


    public StandardRule AddFuzzyRule(string label, string topicName, List<OperatorKeyValueList> pattern, List<RuleModifier> modifiers, List<Statement> statements)
    {
      return AddRule(topicName, topic => new StandardRule(this, label, topic, pattern, modifiers, statements));
    }


    public TopicRule AddTopicRule(string label, string topicName, List<Statement> statements)
    {
      return AddRule(topicName, topic => new TopicRule(this, label, topic, statements));
    }


    protected T AddRule<T>(string topicName, Func<Topic,T> ruleBuilder)
      where T : Rule
    {
      topicName = topicName ?? DefaultTopicName;
      Topic topic = Topics[topicName];

      T r = ruleBuilder(topic);
      Logger.Debug($"Adding rule to topic '{topicName}': {r.ToString()}");

      topic.AddRule(r);
      return r;
    }


    internal void RegisterRuleLabel(string label, Rule r)
    {
      LabelToRuleMap[label] = r;
    }


    public void RegisterEventHandler(string e, List<Statement> statements, IEnumerable<RuleModifier> modifiers)
    {
      Request.EventEnum etype;
      if (Enum.TryParse(e, true, out etype))
      {
        StandardRule rule = new StandardRule(this, statements, modifiers);
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
        if (context.InputContext.Input != null)
        {
          PatternMatchResultList matchingPatterns = PatternManager.CalculateMostLikelyPattern(context.InputContext.Input);

          // Add match values from previous match
          MatchResult previousMatch = context.InputContext.Session.Store[SessionKeys.LatestMatch] as MatchResult;
          if (previousMatch != null && matchingPatterns != null)
          {
            foreach (var curr in matchingPatterns)
            {
              foreach (var prev in previousMatch.Matches)
              {
                if (   curr.MatchValues.ContainsKey(prev.Key) && curr.MatchValues[prev.Key] == null
                    && !curr.HasPatternWithParameter(prev.Key))
                {
                  BotUtility.EvaluationLogger.Debug($"Add previous '{prev.Key}' = '{prev.Value}' to matched pattern.");
                  curr.MatchValues[prev.Key] = prev.Value;
                }
              }
            }
          }

          context.MatchedPatterns = matchingPatterns;

          if (matchingPatterns != null)
            BotUtility.EvaluationLogger.Debug($"Matched patterns: {matchingPatterns.ToString()}");
        }

        string topicName = context.InputContext.Session.CurrentTopic() ?? DefaultTopicName;
        if (Topics.ContainsKey(topicName))
        {
          Topic topic = Topics[topicName];

          // Does user input match anything in current topic?
          SelectReactionsFromTopic(topic, reactions, context, 2.0);

          // No match in current topic - use topic story reaction
          int topicRuleIndex = context.InputContext.Session.GetTopicRuleIndex(topicName);
          if (reactions.Count == 0 && topic.TopicRules.Count > topicRuleIndex)
          {
            foreach (Reaction reaction in topic.TopicRules[topicRuleIndex].CalculateReactions(context, 1.0))
            {
              BotUtility.EvaluationLogger.Debug($"Got '{reaction.Rule}' with score {reaction.Score}");
              reactions.Add(reaction);
            }

            // No topic stories left => clear topic
            if (topicRuleIndex == topic.TopicRules.Count - 1)
              context.InputContext.Session.SetCurrentTopic(null);
          }
        }

        // Try default topic with a smaller weight applied
        Topic nextTopic = null;

        if (topicName != DefaultTopicName)
        {
          if (SelectReactionsFromTopic(DefaultTopic, reactions, context, 1.0))
            nextTopic = DefaultTopic;
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
      foreach (Rule r in topic.StandardRules)
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
        try
        {
          cfg.ParseConfigurationFromFile(this, filename);
        }
        catch (ParserException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Logger.Error(ex);
          ErrorCollection errors = new ErrorCollection { new ErrorCollection.Error(ex.Message, 0, 0) };
          throw new ParserException(filename, errors);
        }
      }
    }
  }
}
