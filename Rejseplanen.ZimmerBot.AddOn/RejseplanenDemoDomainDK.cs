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

      // Expected question: where is a specific train station
      dr.AddRule("find", "station", new WildcardWRegex("s"))
        .Response(c => ProcessorRegistry.Invoke(c, "Fandt '<StopName>'", "Rejseplanen.FindStation", "s"));

      // For debugging ...
      // Should really be implemented as a recursive call to the evaluater with "find station <s>" as input.
      dr.AddRule("fs", new WildcardWRegex("s"))
        .Response(c => ProcessorRegistry.Invoke(c, "Fandt '<StopName>'", "Rejseplanen.FindStation", "s"));

      // Variation of question: missing the station name and asking for it.
      // - A future solution would setup an expectation of some sort of answer (like the station name)
      dr.AddRule("find", "station")
        .Response("Hvilken station?");
    }
  }
}
