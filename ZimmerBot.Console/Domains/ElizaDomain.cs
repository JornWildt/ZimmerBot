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

      dd.AddRule(new RepitionWRegex(new WildcardWRegex()), new ChoiceWRegex(new WordWRegex("far"), new WordWRegex("mor"), "m"), new RepitionWRegex(new WildcardWRegex()))
        .Response(i => ResponseHelper.OneOf(i, "Fortæl mig mere om din <m> ...", "Hvad var dit forhold til din <m>?"));
    }
  }
}
