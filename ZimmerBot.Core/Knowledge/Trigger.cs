using System;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected WRegex Predicate { get; set; }

    protected double PredicateSize { get; set; }


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
      PredicateSize = p.CalculateSize();
    }


    public Trigger(StateWRegex p)
    {
      Predicate = p;
    }


    public WRegex.MatchResult CalculateTriggerScore(EvaluationContext context)
    {
      // FIXME: some mixing of concerns here - should be wrapped differently
      context.CurrentTokenIndex = 0;

      WRegex.MatchResult result = Predicate.CalculateMatchResult(context, new EndOfSequenceWRegex());
      return new WRegex.MatchResult(result, result.Score * PredicateSize);
    }
  }
}
