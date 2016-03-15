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
    {
      Condition.Requires(left, "left").IsNotNull();
      Condition.Requires(right, "right").IsNotNull();

      Left = left;
      Right = right;
    }


    public override WRegex GetLookahead()
    {
      return new ChoiceWRegex(Left.GetLookahead(), Right.GetLookahead());
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      int index = context.CurrentTokenIndex;
      double score1 = Left.CalculateTriggerScore(context, lookahead);
      int index1 = context.CurrentTokenIndex;

      context.CurrentTokenIndex = index;
      double score2 = Right.CalculateTriggerScore(context, lookahead);

      if (score1 >= score2)
      {
        // Restore token index to what it was after score 1 calculation
        context.CurrentTokenIndex = index1;
        return score1;
      }

      return score2;
    }
  }
}
