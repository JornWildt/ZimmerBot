using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Console.Domains
{
  public class SelfDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain sd = kb.NewDomain("Mig selv");

      sd.AddRule("question-what", "ved", "du")
        .SetResponse(i => SelfProcessors.KnownDomains(kb, "Jeg ved lidt om <answer: {a | <a>}; separator=\", \">."));
    }
  }
}
