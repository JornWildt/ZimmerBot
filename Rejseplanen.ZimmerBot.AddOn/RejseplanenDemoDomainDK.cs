using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.WordRegex;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenDemoDomainDK
  {
    // This little domain illustrates how the Rejseplanen addon can be configured for questions about traveling.
    // In Danish, sorry.

    public static void Initialize(KnowledgeBase kb)
    {
      Domain dr = kb.NewDomain("Rejseplanen");

      // Various ways of saying "stoppested"
      dr.DefineWord("station").And("holdeplads").Is("stoppested");
      dr.DefineWord("kører").Is("afgår");

      //// Expected question: where is a specific stoppested
      //dr.AddRule("find", "stoppested", new WildcardWRegex("s"))
      //  .WithResponse(ProcessorRegistry
      //    .BindTo("Rejseplanen.FindStoppested", "s")
      //    .WithTemplate("Fandt '<stopName>'")
      //    .WithTemplate("empty", "Stoppestedet <s> kunne ikke findes") // FIXME: "stoppested" - kan også være "station"
      //    .WithTemplate("error", "Det kan jeg desværre ikke - der er kludder i maskineriet"));

      //// For debugging, shorthand ...
      //// Should really be implemented as a recursive call to the evaluator with "find stoppested <s>" as input.
      //dr.AddRule("fs", new WildcardWRegex("s"))
      //  .WithResponse(ProcessorRegistry
      //    .BindTo("Rejseplanen.FindStoppested", "s")
      //    .WithTemplate("Fandt '<stopName>'")
      //    .WithTemplate("empty", "Stoppestedet <s> kunne ikke findes")
      //    .WithTemplate("error", "Det kan jeg desværre ikke - der er kludder i maskineriet"));

      //// Variation of question: missing the stoppested name and asking for it.
      //dr.AddRule("find", new WordWRegex("stoppested", "s"))
      //  .WithResponse("Hvilken <s>?")
      //  .ExpectAnswer(d =>
      //    d.AddRule("månen")
      //     .WithResponse("Hop i havet ..."))
      //  .ExpectAnswer(d =>
      //    d.AddRule(new WildcardWRegex("s"))
      //     .WithScoreModifier(0.5)
      //     .WithResponse(ProcessorRegistry
      //     .BindTo("Rejseplanen.FindStoppested", "s")
      //     .WithTemplate("Fandt '<stopName>'")
      //     .WithTemplate("empty", "Stoppestedet <s> kunne ikke findes")
      //     .WithTemplate("error", "Det kan jeg desværre ikke - der er kludder i maskineriet")));

      //dr.AddRule("Hvornår", "afgår", "næste", "tog")
      //  .WithResponse("Hvorfra?")
      //  .ExpectAnswer(d =>
      //    d.AddRule(new WildcardWRegex("s"))
      //     .WithResponse(ProcessorRegistry
      //     .BindTo("Rejseplanen.FindStoppested", "s")
      //     .WithTemplate("Fandt '<stopName>'")
      //     .WithTemplate("empty", "Stoppestedet <s> kunne ikke findes")
      //     .WithTemplate("error", "Det kan jeg desværre ikke - der er kludder i maskineriet")));
    }
  }
}
