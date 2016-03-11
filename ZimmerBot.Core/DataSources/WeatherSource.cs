using System;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.DataSources
{
  public static class WeatherSource
  {
    public static Func<string> GetWeatherDescription(Token location, DateTime time, string template)
    {
      Antlr4.StringTemplate.Template t = new Antlr4.StringTemplate.Template(template);

      t.Add("location", location.OriginalText);
      t.Add("time", time);

      return () => t.Render();
    }
  }
}
