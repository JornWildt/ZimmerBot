using System;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected WRegex Regex { get; set; }

    protected double RegexSize { get; set; }

    protected Expression Condition { get; set; }


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

      Regex = p;
      RegexSize = p.CalculateSize();
    }


    public void SetCondition(Expression c)
    {
      Condition = c;
    }


    public WRegex.MatchResult CalculateTriggerScore(EvaluationContext context)
    {
      // FIXME: some mixing of concerns here - should be wrapped differently
      context.CurrentTokenIndex = 0;

      double conditionModifier = 1;

      if (Condition != null)
      {
        object value = Condition.Evaluate(context);
        if (value is bool)
          conditionModifier = ((bool)value) ? 1 : 0;
      }

      WRegex.MatchResult result = Regex.CalculateMatchResult(context, new EndOfSequenceWRegex());

      double totalScore = conditionModifier * result.Score * Math.Max(RegexSize,1);

      return new WRegex.MatchResult(result, totalScore, result.MatchedText);
    }
  }
}
