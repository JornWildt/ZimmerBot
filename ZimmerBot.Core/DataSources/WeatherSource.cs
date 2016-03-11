using System;
using Antlr4.StringTemplate;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.DataSources
{
  public static class WeatherSource
  {
    public static Func<string> GetWeatherDescription(Token location, DateTime time, string template)
    {
      Template t = new Template(template);
      t.Add("location", location.OriginalText);
      t.Add("time", time);

      return () => t.Render();
    }
  }
}
