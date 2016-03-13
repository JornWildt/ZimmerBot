using System;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected string[] Topics { get; set; }


    public Trigger(params string[] topics)
    {
      Topics = topics;
    }


    public double CalculateTriggerScore(ZTokenString input)
    {
      int matches = 0;
      double score = 0;

      for (int i = 0; i < input.Count; ++i)
      {
        bool gotMatch = false;

        for (int j = 0; j < Topics.Length; ++j)
        {
          if (input[i].Matches(Topics[j]))
          {
            gotMatch = true;
            int distance = Math.Abs(j - i);
            if (distance < 3)
              score += 3 - distance;

            //if (ParameterMap.ContainsKey(Matches[j]))
            //  generatorParameters[ParameterMap[Matches[j]]] = input[i].OriginalText;
          }
        }

        if (gotMatch)
          ++matches;
      }

      // Normalize score relative to word count
      score = score / input.Count;

      // Prefer longer matches over short ones
      score += matches;

      if (score < 2)
        return 0;

      return score;
    }
  }
}
