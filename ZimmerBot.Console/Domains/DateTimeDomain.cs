using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Console.Domains
{
  public class DateTimeDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Datoer");

      dd.DefineWord("Mandag").And("Tirsdag").And("Onsday").And("Torsdag").And("Fredag").And("Lørdag").And("Søndag")
        .Is("day").Is("week-day");

      dd.DefineWord("Januar").And("Februar").And("Marts").And("April").And("Maj").And("Juni")
        .And("Juli").And("August").And("September").And("Oktober").And("November").And("December")
        .Is("month");

      dd.AddRule("question-is", "det", "week-day")
        .Describe("Er det <ugedag>")
        .Parameter("week-day")
        .SetResponse(i => DateTimeProcessors.IsItDay(i["week-day"], "<answer>."));

      dd.AddRule("question-which", "dag")
        .SetResponse(i => DateTimeProcessors.ThisDay("I dag er det <answer>"));

      dd.AddRule("question-is", "det", "month")
        .Describe("Er det <måned>")
        .Parameter("month")
        .SetResponse(i => DateTimeProcessors.IsItMonth(i["month"], "Om det er <month>? <answer>."));

      dd.AddRule("question-which", "måned")
        .SetResponse(i => DateTimeProcessors.ThisMonth("I dag er det <answer>"));
    }
  }
}
