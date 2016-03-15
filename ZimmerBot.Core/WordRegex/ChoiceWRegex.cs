using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class ChoiceWRegex : WRegex
  {
    public WRegex Left { get; protected set; }

    public WRegex Right { get; protected set; }


    public ChoiceWRegex(WRegex left, WRegex right)
      : this(left, right, null)
    {
    }

    public ChoiceWRegex(WRegex left, WRegex right, string matchName)
    {
      Condition.Requires(left, "left").IsNotNull();
      Condition.Requires(right, "right").IsNotNull();

      Left = left;
      Right = right;
      MatchName = matchName;
    }


    public override double CalculateSize()
    {
      return Math.Max(Left.CalculateSize(), Right.CalculateSize());
    }


    public override WRegex GetLookahead()
    {
      return new ChoiceWRegex(Left.GetLookahead(), Right.GetLookahead());
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      int index = context.CurrentTokenIndex;
      MatchResult result1 = Left.CalculateMatchResult(context, lookahead);
      int index1 = context.CurrentTokenIndex;

      context.CurrentTokenIndex = index;
      MatchResult result2 = Right.CalculateMatchResult(context, lookahead);

      if (result1.Score >= result2.Score)
      {
        // Restore token index to what it was after score 1 calculation
        context.CurrentTokenIndex = index1;
        return result1.RegisterMatch(MatchName, result1.MatchedText);
      }

      return result2.RegisterMatch(MatchName, result2.MatchedText);
    }
  }
}
