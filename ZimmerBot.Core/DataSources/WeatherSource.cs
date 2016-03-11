using System;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataSources
{
  public static class WeatherSource
  {
    public static Func<string> GetWeatherDescription(Token location, DateTime time, string template)
    {
      string answer = "SOLSKIN";

      return () => TextMerge.MergeTemplate(template, new { location = location.OriginalText, time = time, answer = answer });
    }
  }
}
