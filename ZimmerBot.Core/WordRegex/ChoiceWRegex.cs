using System;
using System.Linq;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class ChoiceWRegex : WRegex
  {
    public List<WRegex> Choices { get; protected set; }


    public ChoiceWRegex()
      : this(Enumerable.Empty<WRegex>())
    {
    }


    public ChoiceWRegex(WRegex left, WRegex right)
      : this(left, right, null)
    {
    }

    public ChoiceWRegex(WRegex left, WRegex right, string matchName)
    {
      Condition.Requires(left, "left").IsNotNull();
      Condition.Requires(right, "right").IsNotNull();

      Choices = new List<WRegex>();
      Choices.Add(left);
      Choices.Add(right);
      MatchName = matchName;
    }


    public ChoiceWRegex(IEnumerable<WRegex> choices)
    {
      Condition.Requires(choices, nameof(choices)).IsNotNull();
      Choices = new List<WRegex>(choices);
    }


    public override double CalculateSize()
    {
      return Choices.Max(c => c.CalculateSize());
    }


    public override WRegex GetLookahead()
    {
      return new ChoiceWRegex(Choices.Select(c => c.GetLookahead()));
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      if (Choices.Count == 0)
        return new MatchResult(0, "");

      MatchResult[] results = new MatchResult[Choices.Count];
      int[] indexes = new int[Choices.Count];

      MatchResult bestResult = null;
      int bestIndex = -1;

      int initialIndex = context.CurrentTokenIndex;

      for (int i = 0; i < Choices.Count; ++i)
      {
        context.CurrentTokenIndex = initialIndex;
        results[i] = Choices[i].CalculateMatchResult(context, lookahead);
        indexes[i] = context.CurrentTokenIndex;

        if (bestResult == null  || results[i].Score >= bestResult.Score)
        {
          bestResult = results[i];
          bestIndex = context.CurrentTokenIndex;
        }
      }

      context.CurrentTokenIndex = bestIndex;
      return bestResult.RegisterMatch(MatchName, bestResult.MatchedText);

      //MatchResult result1 = Choices.CalculateMatchResult(context, lookahead);
      //int index1 = context.CurrentTokenIndex;

      //context.CurrentTokenIndex = index;
      //MatchResult result2 = Right.CalculateMatchResult(context, lookahead);

      //if (result1.Score >= result2.Score)
      //{
      //  // Restore token index to what it was after score 1 calculation
      //  context.CurrentTokenIndex = index1;
      //  return result1.RegisterMatch(MatchName, result1.MatchedText);
      //}

      //return result2.RegisterMatch(MatchName, result2.MatchedText);
    }
  }
}
