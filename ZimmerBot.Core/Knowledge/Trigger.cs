using System;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected string[] Topics { get; set; }

    protected StatePredicate Predicate { get; set; }


    public Trigger(params string[] topics)
    {
      Topics = topics;
    }


    public Trigger(StatePredicate p)
    {
      Predicate = p;
    }


    public double CalculateTriggerScore(EvaluationContext context)
    {
      if (Predicate != null)
      {
        // This is somewhat a hack: we need to rafactor topics and state-predicate into a common "predicat" concept
        return Predicate.CalculateTriggerScore(context);
      }

      if (context.Input.Count == 0)
        return 0;

      int matches = 0;
      double score = 0;

      for (int i = 0; i < context.Input.Count; ++i)
      {
        bool gotMatch = false;

        for (int j = 0; j < Topics.Length; ++j)
        {
          if (context.Input[i].Matches(Topics[j]))
          {
            gotMatch = true;
            int distance = Math.Abs(j - i);
            if (distance < 3)
              score += 3 - distance;
          }
        }

        if (gotMatch)
          ++matches;
      }

      // Normalize score relative to word count
      score = score / context.Input.Count;

      // Prefer longer matches over short ones
      score += matches;

      if (score < 2)
        return 0;

      return score;
    }
  }
}
