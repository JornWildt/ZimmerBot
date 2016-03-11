using System;
using System.Threading;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataSources
{
  public static class DateTimeSource
  {
    public static Func<string> IsItDay(Token day, string template)
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string answer = thisDay.Equals(day.OriginalText, StringComparison.CurrentCultureIgnoreCase)
        ? "Ja" : "Nej";

      return () => TextMerge.MergeTemplate(template, new { day = day.OriginalText, answer = answer });
    }


    public static Func<string> IsItMonth(Token month, string template)
    {
      var thisMonth = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      string answer = thisMonth.Equals(month.OriginalText, StringComparison.CurrentCultureIgnoreCase)
        ? "Ja" : "Nej";

      return () => TextMerge.MergeTemplate(template, new { month = month.OriginalText, answer = answer });
    }
  }
}
