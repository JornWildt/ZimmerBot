using System;
using ZimmerBot.Core.DataSources;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Console.Domains
{
  public class WeatherDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Vejret");
      dd.DefineWord("vejr").And("vejret").Is("weather");

      // "Hvad|hvordan er vejret i Boston"
      dd.AddRule("question", "weather", "location") // FIXME: "location" not defined
        .Describe("Hvad er vejret?")
        .Parameter("location", "location")
        .SetResponse(i => WeatherSource.GetWeatherDescription(i["location"], DateTime.Now, "Vejret i <location> er GODT"));
    }
  }
}
