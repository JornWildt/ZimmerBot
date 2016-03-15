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
      : this(a, null)
    {
    }


    public RepitionWRegex(WRegex a, string matchName)
    {
      Condition.Requires(a, "a").IsNotNull();
      A = a;
      MatchName = matchName;
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
        return new MatchResult(1).RegisterMatch(MatchName, "");

      int startIndex = context.CurrentTokenIndex;
      MatchResult lastResult = null;

      while (context.CurrentTokenIndex < context.Input.Count)
      {
        int index = context.CurrentTokenIndex;

        WRegex lookahead2 = lookahead.GetLookahead();
        MatchResult lookaheadResult = lookahead.CalculateMatchResult(context, lookahead2);

        context.CurrentTokenIndex = index;
        MatchResult result = A.CalculateMatchResult(context, lookahead);

        if (lookaheadResult.Score == 1)
        {
          // Lookahead match => reset token position to position prior to lookahead match and keep lastResult
          context.CurrentTokenIndex = index;
          break;
        }
        else if (result.Score == 1)
        {
          // No lookahead match but match sub-regex => keep token position (as we matched input), store sub-result in lastResult and continue
          lastResult = result;
        }
        else
        {
          // No match in either lookahead or sub-regex => reset token position prior to matching and keep lastResult
          context.CurrentTokenIndex = index;
          break;
        }
      }

      string matchText = "";

      if (!string.IsNullOrEmpty(MatchName) && context.CurrentTokenIndex > startIndex)
      {
        for (int i = startIndex; i < context.CurrentTokenIndex; ++i)
          matchText += (i == startIndex ? "" : " ") + context.Input[i].OriginalText;
      }

      if (lastResult == null)
        return new MatchResult(1).RegisterMatch(MatchName, matchText);
      else
        return lastResult.RegisterMatch(MatchName, matchText);
    }
  }
}
