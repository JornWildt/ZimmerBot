using System;
using System.Threading;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class DateTimeProcessors
  {
    public static Func<string> IsItDay(ZToken day, string template)
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string answer = thisDay.Equals(day.OriginalText, StringComparison.CurrentCultureIgnoreCase)
        ? "Ja" : "Nej";

      return () => TextMerge.MergeTemplate(template, new { day = day.OriginalText, answer = answer });
    }


    public static Func<string> ThisDay(string template)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      return () => TextMerge.MergeTemplate(template, new { answer = answer });
    }


    public static Func<string> Time(string template)
    {
      var answer = DateTime.Now;//.ToShortTimeString();

      return () => TextMerge.MergeTemplate(template, new { answer = answer });
    }


    public static Func<string> IsItMonth(ZToken month, string template)
    {
      var thisMonth = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      string answer = thisMonth.Equals(month.OriginalText, StringComparison.CurrentCultureIgnoreCase)
        ? "Ja" : "Nej";

      return () => TextMerge.MergeTemplate(template, new { month = month.OriginalText, answer = answer });
    }


    public static Func<string> ThisMonth(string template)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      return () => TextMerge.MergeTemplate(template, new { answer = answer });
    }
  }
}
