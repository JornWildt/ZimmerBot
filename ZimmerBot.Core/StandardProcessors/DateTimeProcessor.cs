using System;
using System.Collections.Generic;
using System.Threading;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.StandardProcessors
{
  public static class DateTimeProcessor
  {
    public static void Initialize()
    {
      ProcessorRegistry.RegisterProcessor("DateTime.IsItWeekDay", IsItWeekDay);
      ProcessorRegistry.RegisterProcessor("DateTime.IsItMonth", IsItMonth);
      ProcessorRegistry.RegisterProcessor("DateTime.Time", Time);
    }


    public static ProcessorOutput IsItWeekDay(ProcessorInput input)
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string day = input.GetParameter<string>(0);
      bool answer = thisDay.Equals(day, StringComparison.CurrentCultureIgnoreCase);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["day"] = day;
      result["answer"] = answer;
      return new ProcessorOutput(result);
    }


    public static ProcessorOutput Time(ProcessorInput input)
    {
      var answer = DateTime.Now;//.ToShortTimeString();

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["answer"] = answer;
      return new ProcessorOutput(result);
    }


    public static ProcessorOutput IsItMonth(ProcessorInput input)
    {
      var thisMonth = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      string month = input.GetParameter<string>(0);
      bool answer = thisMonth.Equals(month, StringComparison.CurrentCultureIgnoreCase);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["month"] = month;
      result["answer"] = answer;
      return new ProcessorOutput(result);
    }
  }
}
