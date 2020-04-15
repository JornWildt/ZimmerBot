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
      ProcessorRegistry.RegisterProcessor("DateTime.IsItWeekend", IsItWeekend);
      ProcessorRegistry.RegisterProcessor("DateTime.IsItMonth", IsItMonth);
      ProcessorRegistry.RegisterProcessor("DateTime.Time", Time);
      ProcessorRegistry.RegisterProcessor("DateTime.Details", Details);
    }


    public static ProcessorOutput Time(ProcessorInput input)
    {
      var answer = DateTime.Now;//.ToShortTimeString();

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["answer"] = answer;
      return new ProcessorOutput(result);
    }


    public static ProcessorOutput Details(ProcessorInput input)
    {
      var now = DateTime.Now;
      var weekDay = now.DayOfWeek;

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["now"] = now;
      result["isMonday"] = weekDay == DayOfWeek.Monday;
      result["isTuesday"] = weekDay == DayOfWeek.Tuesday;
      result["isWednesday"] = weekDay == DayOfWeek.Wednesday;
      result["isThursday"] = weekDay == DayOfWeek.Thursday;
      result["isFriday"] = weekDay == DayOfWeek.Friday;
      result["isSaturday"] = weekDay == DayOfWeek.Saturday;
      result["isSunday"] = weekDay == DayOfWeek.Monday;
      result["isWeekend"] = weekDay == DayOfWeek.Saturday || weekDay == DayOfWeek.Sunday;
      result["isMorning"] = now.Hour >= 5 && now.Hour < 9;
      result["isNoon"] = now.Hour >= 11 && now.Hour < 13;
      result["isAfternoon"] = now.Hour >= 13 && now.Hour <= 17;
      result["isEvening"] = now.Hour > 17 && now.Hour <= 21;
      result["isNight"] = now.Hour > 21 || now.Hour < 5;

      return new ProcessorOutput(result);
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


    public static ProcessorOutput IsItWeekend(ProcessorInput input)
    {
      bool answer = DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday;

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
