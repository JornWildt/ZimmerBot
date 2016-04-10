using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  /// <summary>
  /// Represents zero or more repetisions of a predicate
  /// </summary>
  public class RepetitionWRegex : WRegex
  {
    public WRegex A { get; protected set; }

    public int MinCount { get; protected set; }

    public int MaxCount { get; protected set; }


    public RepetitionWRegex(WRegex a)
      : this(a, 0, 9999, null)
    {
    }


    public RepetitionWRegex(WRegex a, int min, int max)
      : this(a, min, max, null)
    {
    }


    public RepetitionWRegex(WRegex a, string matchName)
      : this(a, 0, 9999, matchName)
    {
    }


    public RepetitionWRegex(WRegex a, int min, int max, string matchName)
    {
      Condition.Requires(a, "a").IsNotNull();
      A = a;
      MatchName = matchName;
      MinCount = min;
      MaxCount = max;
    }


    public override double CalculateSize()
    {
      return A.CalculateSize();
    }


    public override WRegex GetLookahead()
    {
      return A;
    }


    public override MatchResult CalculateMatchResult(TriggerEvaluationContext context, WRegex lookahead)
    {
      if (context.CurrentTokenIndex >= context.InputContext.Input.Count)
      {
        // Empty sequence is a match if no minimum count is specified
        if (MinCount == 0)
          return new MatchResult(1, "").RegisterMatch(MatchName, "");
        else
          return new MatchResult(0, "").RegisterMatch(MatchName, "");
      }

      string matchedText = "";
      int startIndex = context.CurrentTokenIndex;
      MatchResult lastResult = null;

      while (context.CurrentTokenIndex < context.InputContext.Input.Count)
      {
        int index = context.CurrentTokenIndex;

        WRegex lookahead2 = lookahead.GetLookahead();
        MatchResult lookaheadResult = lookahead.CalculateMatchResult(context, lookahead2);

        context.CurrentTokenIndex = index;
        MatchResult result = A.CalculateMatchResult(context, lookahead);

        if (lookaheadResult.Score > 0.999)
        {
          // Lookahead match => reset token position to position prior to lookahead match and keep lastResult
          context.CurrentTokenIndex = index;
          break;
        }
        else if (result.Score > 0.999)
        {
          // No lookahead match but match sub-regex => keep token position (as we matched input), store sub-result in lastResult and continue
          lastResult = result;
          matchedText += (matchedText.Length == 0 ? "" : " ") + result.MatchedText;
        }
        else
        {
          // No match in either lookahead or sub-regex => reset token position prior to matching and keep lastResult
          context.CurrentTokenIndex = index;
          break;
        }
      }

      if (context.CurrentTokenIndex - startIndex < MinCount || context.CurrentTokenIndex - startIndex > MaxCount)
      {
        // Did not match the correct number of inputs => reset as no words were matched and return empty result
        context.CurrentTokenIndex = startIndex;

        return new MatchResult(0, "")
          .RegisterMatch((context.CurrentRepetitionIndex++).ToString(), "")
          .RegisterMatch(MatchName, "");
      }

      if (lastResult == null)
        return new MatchResult(1, matchedText)
          .RegisterMatch((context.CurrentRepetitionIndex++).ToString(), matchedText)
          .RegisterMatch(MatchName, matchedText);
      else
        return lastResult
          .RegisterMatch((context.CurrentRepetitionIndex++).ToString(), matchedText)
          .RegisterMatch(MatchName, matchedText);
    }
  }
}
