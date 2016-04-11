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
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class KnowledgeBase
  {
    static ILog Logger = LogManager.GetLogger(typeof(KnowledgeBase));


    public enum InitializationMode { Clear, Restore, RestoreIfExists }

    public RDFStore MemoryStore { get; protected set; }

    public Dictionary<string, Concept> Concepts { get; protected set; }

    public List<Rule> Rules { get; protected set; }

    public Dictionary<Request.EventEnum, Rule> EventHandlers { get; protected set; }

    public Pipeline<InputPipelineItem> InputPipeline { get; protected set; }


    public KnowledgeBase()
      : this("default")
    {
    }


    public KnowledgeBase(string memoryId)
    {
      MemoryStore = new RDFStore(memoryId);
      Concepts = new Dictionary<string, Concept>();
      Rules = new List<Rule>();
      EventHandlers = new Dictionary<Request.EventEnum, Rule>();
      InputPipeline = new Pipeline<InputPipelineItem>();
      InputPipeline.AddHandler(new WordTaggingStage());
      InputPipeline.AddHandler(new SentenceTaggingStage());
      InputPipeline.AddHandler(new ReactionGeneratorStage());
      InputPipeline.AddHandler(new OutputGeneratorStage());
      InputPipeline.AddHandler(new ChatLoggerStage());
    }


    public void Initialize(InitializationMode mode)
    {
      MemoryStore.Initialize(mode);
    }


    public Concept AddConcept(string name, List<List<string>> patterns)
    {
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      Concept w = new Concept(this, name, patterns);
      Concepts.Add(name, w);
      return w;
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


    public void RegisterEventHandler(string e, List<Statement> statements)
    {
      Request.EventEnum etype;
      if (Enum.TryParse(e, true, out etype))
      {
        Rule rule = new Rule(this);
        rule.WithOutputStatements(statements);
        EventHandlers[etype] = rule;
      }
      else
        throw new ApplicationException($"Unknown event '{e}'");
    }


    public ReactionSet FindMatchingReactions(TriggerEvaluationContext context, ReactionSet reactions = null)
    {
      if (reactions == null)
        reactions = new ReactionSet();

      if (context.InputContext.Request.EventType != null)
      {
        if (EventHandlers.ContainsKey(context.InputContext.Request.EventType.Value))
        {
          Rule rule = EventHandlers[context.InputContext.Request.EventType.Value];
          ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new WRegex.MatchResult(1, ""));
          Reaction reaction = new Reaction(rc, rule);
          reactions.Add(reaction);
        }
      }
      else
      {
        foreach (Rule r in Rules)
        {
          Reaction reaction = r.CalculateReaction(context);
          if (reaction != null)
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
