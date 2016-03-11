using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Responses;


namespace ZimmerBot.Console.Domains
{
  public class SelfDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain sd = kb.NewDomain("Mig selv");

      sd.AddRule("question-what", "do", "you", "know")
        .SetResponse(new DomainKnowledgeResponseGenerator(kb));
    }
  }
}
