using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Console.Domains
{
  public class ElizaDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Psychotherapy");

      dd.AddRule("jeg", "har", "brug", "for", new RepitionWRegex(new WildcardWRegex(), "x"))
        .Response("Hvorfor? Gør <x> dig glad?");
    }
  }
}
