using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class WRegex
  {
    public WRegexBase Expr { get; protected set; }

    protected NFANode Root { get; set; }


    public Type TypeOfExpr { get { return Expr.GetType(); } }


    public WRegex(WRegexBase expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      Expr = expr;
    }


    public double CalculateSize()
    {
      return Expr.CalculateSize();
    }


    public MatchResult CalculateMatch(TriggerEvaluationContext context)
    {
      if (Root == null)
      {
        Root = Expr.CalculateNFA(context);
      }

      MatchResult result = Expr.CalculateNFAMatch(Root, context);
      return result;
    }
  }
}
