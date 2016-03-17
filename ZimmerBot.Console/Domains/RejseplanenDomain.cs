using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Console.Domains
{
  public static class RejseplanenDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dr = kb.NewDomain("Rejseplanen");

      dr.AddRule("find", "station", new WildcardWRegex("s"))
        .Response(m => ProcessorRegistry.Invoke("Rejseplanen.FindStation", new ProcessorRegistry.ProcessorInput(m.Matches["s"])));

      //dd.AddRule("question", new RepitionWRegex(new WildcardWRegex()), "vejret", new RepitionWRegex(new WildcardWRegex()), new WordWRegex("location", "l"))
      //  .Response(i => WeatherProcessor.GetWeatherDescription(i.Matches["l"], DateTime.Now, "Vejret omkring <location> er <answer>"));
    }
  }
}
