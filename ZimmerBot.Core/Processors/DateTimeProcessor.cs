using System;
using System.Threading;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class DateTimeProcessors
  {
    public static void RegisterProcessors()
    {
      ProcessorRegistry.RegisterProcessor("DateTime.IsItWeekDay", IsItWeekDay);
    }


    public static object IsItWeekDay(ProcessorInput input)
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string inpDay = input.GetParameter<string>(0);
      bool answer = thisDay.Equals(inpDay, StringComparison.CurrentCultureIgnoreCase);

      return new { day = inpDay, answer = answer }; // TextMerge.MergeTemplate(template, new { day = day.OriginalText, answer = answer });
    }


    public static Func<object> ThisDay(string template)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      return () => new { answer = answer };// TextMerge.MergeTemplate(template, new { answer = answer });
    }


    public static Func<object> Time(string template)
    {
      var answer = DateTime.Now;//.ToShortTimeString();

      return () => new { answer = answer }; // TextMerge.MergeTemplate(template, new { answer = answer });
    }


    public static Func<object> IsItMonth(ZToken month, string template)
    {
      var thisMonth = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      bool answer = thisMonth.Equals(month.OriginalText, StringComparison.CurrentCultureIgnoreCase);

      return () => new { month = month.OriginalText, answer = answer }; // TextMerge.MergeTemplate(template, new { month = month.OriginalText, answer = answer });
    }


    public static Func<object> ThisMonth(string template)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      return () => new { answer = answer };//TextMerge.MergeTemplate(template, new { answer = answer });
    }
  }
}
