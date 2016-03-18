using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenDemoDomainDK
  {
    // This little domain illustrates how the Rejseplanen addon can be configured for questions about traveling.
    // - In Danish, sorry.

    public static void Initialize(KnowledgeBase kb)
    {
      Domain dr = kb.NewDomain("Rejseplanen");

      // Various ways of saying "stoppested"
      dr.DefineWord("station").And("holdeplads").Is("stoppested");

      // Expected question: where is a specific stoppested
      dr.AddRule("find", "stoppested", new WildcardWRegex("s"))
        .Response(c => ProcessorRegistry.Invoke(c, "Fandt '<StopName>'", "Rejseplanen.FindStoppested", "s"));

      // For debugging, shorthand ...
      // Should really be implemented as a recursive call to the evaluater with "find stoppested <s>" as input.
      dr.AddRule("fs", new WildcardWRegex("s"))
        .Response(c => ProcessorRegistry.Invoke(c, "Fandt '<StopName>'", "Rejseplanen.FindStoppested", "s"));

      // Variation of question: missing the stoppested name and asking for it.
      // - A future solution would setup an expectation of some sort of answer (like the stoppested name)
      dr.AddRule("find", new WordWRegex("stoppested", "s"))
        .Response("Hvilken <s>?");
    }
  }
}
