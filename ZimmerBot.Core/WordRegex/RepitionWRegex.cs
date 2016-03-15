using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  /// <summary>
  /// Represents zero or more repetisions of a predicate
  /// </summary>
  public class RepitionWRegex : WRegex
  {
    public WRegex A { get; protected set; }


    public RepitionWRegex(WRegex a)
    {
      Condition.Requires(a, "a").IsNotNull();
      A = a;
    }


    public override double CalculateSize()
    {
      return A.CalculateSize();
    }


    public override WRegex GetLookahead()
    {
      return A;
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      // Empty sequnce is a match
      if (context.CurrentTokenIndex >= context.Input.Count)
        return new MatchResult(1);

      int currentTokenIndex = context.CurrentTokenIndex;

      while (context.CurrentTokenIndex < context.Input.Count)
      {
        int index = context.CurrentTokenIndex;

        WRegex lookahead2 = lookahead.GetLookahead();
        MatchResult lookaheadResult = lookahead.CalculateMatchResult(context, lookahead2);

        context.CurrentTokenIndex = index;
        MatchResult result = A.CalculateMatchResult(context, lookahead);

        if (result.Score < 1 || lookaheadResult.Score == 1)
        {
          context.CurrentTokenIndex = index;
          break;
        }
      }

      return new MatchResult(1);
    }
  }
}
