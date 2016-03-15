using System;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected string[] Topics { get; set; }

    protected WRegex Predicate { get; set; }


    public Trigger(params object[] topics)
    {
      SequenceWRegex p = new SequenceWRegex();

      foreach (object t in topics)
      {
        if (t is string)
          p.Add(new WordWRegex((string)t));
        else if (t is WRegex)
          p.Add((WRegex)t);
        else if (t == null)
          throw new ArgumentNullException("t", "Null item in topics");
        else
          throw new InvalidOperationException(string.Format("Cannot add {0} ({1} as trigger predicate.", t, t.GetType()));
      }

      Predicate = p;
    }


    public Trigger(StateWRegex p)
    {
      Predicate = p;
    }


    public double CalculateTriggerScore(EvaluationContext context)
    {
      if (Predicate != null)
      {
        // FIXME: some mixing of concerns here - should be wrapped differently
        context.CurrentTokenIndex = 0;

        // FIXME: need only be calculated once and for all!
        double size = Predicate.CalculateSize();

        return Predicate.CalculateTriggerScore(context, new EndOfSequenceWRegex()) * size;

        // This is somewhat a hack: we need to rafactor topics and state-predicate into a common "predicat" concept
        //return 0; // FIXME  Predicate.CalculateTriggerScore(context);
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
