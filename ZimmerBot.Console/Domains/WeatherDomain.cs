using System;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Console.Domains
{
  public class WeatherDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Vejret");
      dd.DefineWord("vejr").And("vejret");

      // "Hvad|hvordan er vejret i Boston"
      dd.AddRule("question", new RepitionWRegex(new WildcardWRegex()), "vejret", new RepitionWRegex(new WildcardWRegex()), new WordWRegex("location", "l"))
        .Response(i => WeatherProcessor.GetWeatherDescription(i.Matches["l"], DateTime.Now, "Vejret omkring <location> er <answer>"));
    }
  }
}
