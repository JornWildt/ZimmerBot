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

      dd.AddRule(new ChoiceWRegex(new WordWRegex("hej"), new ChoiceWRegex(new WordWRegex("dav"), new WordWRegex("goddag"))))
        .WithResponse(ResponseHelper.OneOf(
          "Velkommen. Hvad kan jeg hjælpe dig med?"));

      dd.AddRule("jeg", "har", "brug", "for", new RepetitionWRegex(new WildcardWRegex(), "x"))
        .WithResponse("Hvorfor? Gør <x> dig glad?");

      dd.AddRule(new RepetitionWRegex(new WildcardWRegex()), new ChoiceWRegex(new WordWRegex("far"), new WordWRegex("mor"), "m"), new RepetitionWRegex(new WildcardWRegex()))
        .WithResponse(ResponseHelper.OneOf(
          "Fortæl mig mere om din <m> ...",
          "Hvad var dit forhold til din <m>?",
          "Hvad føler du i forhold til din <m>?",
          "Hvordan forholder det sig til dine følelser i dag?",
          "Ja, gode familierelationer er vigtige."));

      dd.AddRule(new RepetitionWRegex(new WildcardWRegex()), new ChoiceWRegex(new WordWRegex("barn"), new WordWRegex("barndom"), "m"), new RepetitionWRegex(new WildcardWRegex()))
        .WithResponse(ResponseHelper.OneOf(
          "Havde du nogle nære venner som barn?",
          "Hvad er din ynglingserindring fra din barndom?",
          "Blev du ind i mellem drillet af de andre børn?",
          "Hvordan tror du at dine barndomsoplevelser har påvirket dig i dag?"));

      dd.AddRule("du", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Nu snakker vi om dig. Prøv igen",
          "Hvorfor siger du det om mig?",
          "Hvad får dig til at sige det?",
          "Hvorfor bekymrer du dig om det?"));

      dd.AddRule("jeg", "er", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Hvorfor tror du at du er <m>?",
          "Søger du råd hos mig fordi du er <m>?",
          "Hvor lang tid har du været det?",
          "Hvordan har du det med at være <m>?",
          "Hvad får dig til at sige at du er <m>?"));

      dd.AddRule("hvorfor", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Ja, fortæl mig hvorfor?",
          "Hvad tror du?",
          "Hvad får dig til at spørge om det?",
          "Hvor lang tid har du været?"));

      dd.AddRule("tak", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Selv tak",
          "Velbekomme",
          "Godt. Gjorde det dig glad?"));

      dd.AddRule("er", "du", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Hvorfor er det vigtigt om jeg er <m>?",
          "Ville du foretrække det hvis jeg ikke var <m>?",
          "Måske tror du at jeg er <m>?",
          "Jeg kunne godt være <m> - hvad tror du?",
          "Jeg kunne godt være <m> - hvad ville det betyde for dig?"));

      dd.AddRule("jeg", "vil", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Hvad ville det betyde hvis du <m>?",
          "Hvorfor vil du det?",
          "Hvad ville du gøre hvis det ikke skete?",
          "Hvis det lykkedes, hvad ville du så gøre?"));

      dd.AddRule("jeg", "har", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Hvorfor fortæller du mig at du har <m>?",
          "Virkeligt? Hvorfor?",
          "Nu hvor du har det, hvad vil du så gøre nu?"));

      dd.AddRule("fordi", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Er det virkelig den rigtige årsag?",
          "Kan du komme på andre årsager? Hvilke?",
          "Kan det være årsag til andre ting?",
          "Fordi hvad?",
          "Kan du uddybe det?"));

      dd.AddRule("kan", "du", new RepetitionWRegex(new WildcardWRegex(), "m"))
        .WithResponse(ResponseHelper.OneOf(
          "Hvad får dig til at tro at jeg kan det?",
          "Hvis jeg kunne <m> hvad så?",
          "Hvorfor spørger du om jeg kan <m>?"));

      SequenceWRegex r = new SequenceWRegex();
      r.Add(new WildcardWRegex());
      r.Add(new RepetitionWRegex(new WildcardWRegex()));

      dd.AddRule(r)
        .WithScoreModifier(0.25)
        .WithResponse(ResponseHelper.OneOf(
          "Hmmm, fortæl mig noget mere om det?",
          "Okay. Lad os skifte fokus lidt ... fortæl mig om din familie?",
          "Kan du uddybe det?",
          "Hvad får dig til at sige det?",
          "Aha ...",
          "Javel ...",
          "Okay ... og hvad så?",
          "Meget interessant.",
          "Okay. Og hvad fortæller det så dig?",
          "Hvilke følelser mærker du når du siger det?"));

      // Startup rule
      dd.AddRule()
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
        .WithResponse("Hej. Mit navn er Eliza.");

      // Startup rule
      dd.AddRule()
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
        .WithResponse("Jeg er en chatbot med speciale i psykiatri. Hvad kan jeg hjælpe dig med?");

      // FIXME: this rules should not fire when someone has just said something. It should wait 30 sec. after last entry.
      dd.AddRule()
        .WithSchedule(TimeSpan.FromSeconds(30))
        .WithCondition(new FunctionCallExpr("probability", new ConstantNumberExpr(0.33)))
        .WithResponse(ResponseHelper.OneOf(
          "Nåh... ?",
          "Hmmm ...",
          "Fortæl mere?",
          "Er du der?"));
    }
  }
}
