using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Console.Domains
{
  public static class RejseplanenDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dr = kb.NewDomain("Rejseplanen");

      dr.AddRule("find", "station", new WildcardWRegex("s"))
        .Response(c => ProcessorRegistry.Invoke(c, "Rejseplanen.FindStation", "s"));

      dr.AddRule("find", "station")
        .Response("Hvilken station?");
    }
  }
}
