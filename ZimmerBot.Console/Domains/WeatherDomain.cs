using System;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Console.Domains
{
  public class WeatherDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Vejret");
      dd.DefineWord("vejr").And("vejret").Is("weather");

      // "Hvad|hvordan er vejret i Boston"
      //dd.AddRule("question", "weather", "location")
      //  .Describe("Hvad er vejret?")
      //  .Parameter("location")
      //  .Response(i => WeatherProcessors.GetWeatherDescription(i["location"], DateTime.Now, "Vejret omkring <location> er <answer>"));
    }
  }
}
