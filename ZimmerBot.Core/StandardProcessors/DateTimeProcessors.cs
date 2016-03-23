﻿using System;
using System.Collections.Generic;
using System.Threading;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.StandardProcessors
{
  public static class DateTimeProcessors
  {
    public static void Initialize()
    {
      ProcessorRegistry.RegisterProcessor("DateTime.IsItWeekDay", IsItWeekDay);
      ProcessorRegistry.RegisterProcessor("DateTime.ThisDay", ThisDay);
      ProcessorRegistry.RegisterProcessor("DateTime.ThisMonth", ThisMonth);
      ProcessorRegistry.RegisterProcessor("DateTime.IsItMonth", IsItMonth);
      ProcessorRegistry.RegisterProcessor("DateTime.Time", Time);
    }


    public static object IsItWeekDay(ProcessorInput input)
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string day = input.GetParameter<string>(0);
      bool answer = thisDay.Equals(day, StringComparison.CurrentCultureIgnoreCase);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["day"] = day;
      result["answer"] = answer;
      return result;
    }


    public static object ThisDay(ProcessorInput input)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["answer"] = answer;
      return result;
    }


    public static object Time(ProcessorInput input)
    {
      var answer = DateTime.Now;//.ToShortTimeString();

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["answer"] = answer;
      return result;
    }


    public static object IsItMonth(ProcessorInput input)
    {
      var thisMonth = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      string month = input.GetParameter<string>(0);
      bool answer = thisMonth.Equals(month, StringComparison.CurrentCultureIgnoreCase);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["month"] = month;
      result["answer"] = answer;
      return result;
    }


    public static object ThisMonth(ProcessorInput input)
    {
      var answer = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["answer"] = answer;
      return result;
    }
  }
}
