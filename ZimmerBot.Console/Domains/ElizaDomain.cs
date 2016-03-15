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
        .Response(i => ResponseHelper.OneOf(i, 
          "Fortæl mig mere om din <m> ...", 
          "Hvad var dit forhold til din <m>?", 
          "Hvad føler du i forhold til din <m>?",
          "Hvordan forholder det sig til dine følelser i dag?",
          "Ja, gode familierelationer er vigtige."));

      dd.AddRule(new RepitionWRegex(new WildcardWRegex()), new ChoiceWRegex(new WordWRegex("barn"), new WordWRegex("barndom"), "m"), new RepitionWRegex(new WildcardWRegex()))
        .Response(i => ResponseHelper.OneOf(i,
          "Havde du nogle nære venner som barn?",
          "Hvad er din ynglingserindring fra din barndom?",
          "Blev du ind i mellem drillet af de andre børn?",
          "Hvordan tror du at dine barndomsoplevelser har påvirket dig i dag?"));
    }
  }
}
