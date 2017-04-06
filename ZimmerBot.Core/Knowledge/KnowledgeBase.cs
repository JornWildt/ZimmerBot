using System;
using System.Collections.Generic;
using System.IO;
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


    public enum InitializationMode { Clear, Restore, RestoreIfExists }

    public RDFStore MemoryStore { get; protected set; }

    public IDictionary<string, Concept> Concepts { get; protected set; }

    public IDictionary<string, Entity> Entities { get; protected set; }

    public IList<Rule> Rules { get; protected set; }

    public IDictionary<Request.EventEnum, List<Rule>> EventHandlers { get; protected set; }

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
      Entities = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
      Rules = new List<Rule>();
      EventHandlers = new Dictionary<Request.EventEnum, List<Rule>>();
      LabelToRuleMap = new Dictionary<string, Rule>();
      SparqlForEntities = new List<string>();

      InputPipeline = new Pipeline<InputPipelineItem>();
      InputPipeline.AddHandler(new WordTaggingStage());
      InputPipeline.AddHandler(new EntityTaggingStage());
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
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      Concept w = new Concept(this, name, patterns);
      Concepts.Add(name, w);
      return w;
    }


    public Entity AddEntity(string label)
    {
      Entity e = new Entity(label);
      Entities[e.Label] = e;
      return e;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      foreach (Rule r in Rules)
      {
        r.RegisterScheduledJobs(scheduler, botId);
      }
    }


    public Rule AddRule(params object[] topics)
    {
      Rule r = new Rule(this, topics);
      Rules.Add(r);
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
        Rule rule = new Rule(this);
        rule.WithStatements(statements);
        if (!EventHandlers.ContainsKey(etype))
          EventHandlers[etype] = new List<Rule>();
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
          foreach (Rule rule in EventHandlers[context.InputContext.Request.EventType.Value])
          {
            ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(1));
            Reaction reaction = new Reaction(rc, rule, null);
            reactions.Add(reaction);
          }
        }
      }
      else
      {
        foreach (Rule r in Rules)
        {
          IList<Reaction> result = r.CalculateReaction(context);
          if (result != null)
            foreach (Reaction reaction in result)
              reactions.Add(reaction);
        }
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
