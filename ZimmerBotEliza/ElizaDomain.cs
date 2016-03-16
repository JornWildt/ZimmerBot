using System;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBotEliza
{
  public class ElizaDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Psykoterapi");

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

      dd.AddRule("du", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Nu snakker vi om dig. Prøv igen",
          "Hvorfor siger du det om mig?",
          "Hvad får dig til at sige det?",
          "Hvorfor bekymrer du dig om det?"));

      dd.AddRule("jeg", "er", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Hvorfor tror du at du er <m>?",
          "Søger du råd hos mig fordi du er <m>?",
          "Hvor lang tid har du været det?",
          "Hvordan har du det med at være <m>?",
          "Hvad får dig til at sige at du er <m>?"));

      dd.AddRule("hvorfor", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Ja, fortæl mig hvorfor?",
          "Hvad tror du?",
          "Hvad får dig til at spørge om det?",
          "Hvor lang tid har du været?"));

      dd.AddRule("er", "du", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Hvorfor er det vigtigt om jeg er <m>?",
          "Ville du foretrække det hvis jeg ikke var <m>?",
          "Måske tror du at jeg er <m>?",
          "Jeg kunne godt være <m> - hvad tror du?",
          "Jeg kunne godt være <m> - hvad ville det betyde for dig?"));

      dd.AddRule("jeg", "vil", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i, 
          "Hvad ville det betyde hvis du <m>?",
          "Hvorfor vil du det?",
          "Hvad ville du gøre hvis det ikke skete?",
          "Hvis det lykkedes, hvad ville du så gøre?"));

      dd.AddRule("fordi", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Er det virkelig den rigtige årsag?",
          "Kan du komme på andre årsager? Hvilke?",
          "Kan det være årsag til andre ting?"));

      dd.AddRule("kan", "du", new RepitionWRegex(new WildcardWRegex(), "m"))
        .Response(i => ResponseHelper.OneOf(i,
          "Hvad får dig til at tro at jeg kan det?",
          "Hvis jeg kunne <m> hvad så?",
          "Hvorfor spørger du om jeg kan <m>?"));

      SequenceWRegex r = new SequenceWRegex();
      r.Add(new WildcardWRegex());
      r.Add(new RepitionWRegex(new WildcardWRegex()));

      dd.AddRule(r)
        .Response(i => ResponseHelper.OneOf(i,
          "Hmmm, fortæl mig noget mere om det?",
          "Okay. Lad os skifte fokus lidt ... fortæl mig om din familie?",
          "Kan du uddybe det?",
          "Hvad får dig til at sige det?",
          "Aha ...",
          "Javel ...",
          "Okay ...",
          "Meget interessant.",
          "Okay. Og hvad fortæller det så dig?",
          "Hvilke følelser mærker du når du siger det?"));

      // Startup rule
      dd.AddRule()
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
        .Response("Hej. Mit navn er Eliza. Jeg er et chatbot.");

      dd.AddRule()
        .WithSchedule(TimeSpan.FromSeconds(30))
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.NotEquals))
        .Response(i => ResponseHelper.OneOf(i, 
          "Nåh... ?",
          "Hmmm ...",
          "Fortæl mere?",
          "Er du der?"));
    }
  }
}
