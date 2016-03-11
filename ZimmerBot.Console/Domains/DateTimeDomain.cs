using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Responses;


namespace ZimmerBot.Console.Domains
{
  public class DateTimeDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Datoer");
      dd.DefineWord("Mandag").And("Tirsdag").And("Onsday").And("Torsdag").And("Fredag").And("Lørdag").And("Søndag").Is("day").Is("week-day");

      dd.AddRule("question-is", "det", "week-day")
        .Describe("Hvilken ugedag er det?")
        .Parameter("name", "week-day")
        .SetResponse(new WeekDayResponseGenerator("what-day-is-it"));
    }
  }
}
