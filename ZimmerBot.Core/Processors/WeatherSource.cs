using System;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class WeatherProcessors
  {
    public static Func<string> GetWeatherDescription(object location, DateTime time, string template)
    {
      // FIXME: this is only a skeleton of how it could be implemented. 
      // - Answer should be found by API lookup.
      string answer = "SOLSKIN";

      return () => TextMerge.MergeTemplate(template, new { location = location, time = time, answer = answer });
    }
  }
}
