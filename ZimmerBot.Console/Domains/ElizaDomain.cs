using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Console.Domains
{
  public class ElizaDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Psychotherapy");

      dd.AddRule("behøver", new RepitionWRegex(new WildcardWRegex()))
        .Response("NÅH HAR DU DET");
    }
  }
}
