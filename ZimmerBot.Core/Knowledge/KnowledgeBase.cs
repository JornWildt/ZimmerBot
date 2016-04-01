using System.Collections.Generic;
using System.IO;
using log4net;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public class KnowledgeBase
  {
    static ILog Logger = LogManager.GetLogger(typeof(KnowledgeBase));

    protected List<Domain> Domains { get; set; }

    public RDFStore MemoryStore { get; protected set; }

    public Pipeline<InputPipelineItem> InputPipeline { get; protected set; }


    public KnowledgeBase()
    {
      Domains = new List<Domain>();
      MemoryStore = new RDFStore();
      InputPipeline = new Pipeline<InputPipelineItem>();
      InputPipeline.AddHandler(new WordTaggingStage());
      InputPipeline.AddHandler(new SentenceTaggingStage());
      InputPipeline.AddHandler(new ReactionGeneratorStage());
    }


    public Domain NewDomain(string name)
    {
      Domain d = new Domain(this, name);
      Domains.Add(d);
      return d;
    }


    public IEnumerable<Domain> GetDomains()
    {
      return Domains;
    }


    public void ExpandTokens(ZTokenSequence input)
    {
      foreach (Domain d in Domains)
      {
        d.ExpandTokens(input);
      }
    }


    public ReactionSet FindMatchingReactions(EvaluationContext context, ReactionSet reactions)
    {
      if (reactions == null)
        reactions = new ReactionSet();

      foreach (Domain d in Domains)
        d.FindMatchingReactions(context, reactions);

      return reactions;
    }


    public void LoadFromFiles(string path, string pattern = "*.zbot", SearchOption option = SearchOption.TopDirectoryOnly)
    {
      ConfigurationParser cfg = new ConfigurationParser();

      foreach (string filename in Directory.EnumerateFiles(path, pattern, option))
      {
        Logger.InfoFormat("Loading zbot file: {0}", filename);
        Domain d = NewDomain(Path.GetFileName(filename));
        cfg.ParseConfigurationFromFile(d, filename);
      }
    }


    public static KnowledgeBase CreateFromFiles(string path, string pattern = "*.zbot", SearchOption option = SearchOption.TopDirectoryOnly)
    {
      KnowledgeBase kb = new KnowledgeBase();
      kb.LoadFromFiles(path, pattern, option);
      return kb;
    }
  }
}
