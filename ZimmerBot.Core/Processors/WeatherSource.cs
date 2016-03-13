using System;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class WeatherProcessors
  {
    public static Func<string> GetWeatherDescription(ZToken location, DateTime time, string template)
    {
      string answer = "SOLSKIN";

      return () => TextMerge.MergeTemplate(template, new { location = location.OriginalText, time = time, answer = answer });
    }
  }
}
