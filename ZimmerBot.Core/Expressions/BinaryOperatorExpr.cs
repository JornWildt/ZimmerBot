using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class BinaryOperatorExpr : Expression
  {
    public enum OperatorType { Equals }

    public Expression Left { get; protected set; }

    public Expression Right { get; protected set; }

    public OperatorType Operator { get; protected set; }


    public BinaryOperatorExpr(Expression left, Expression right, OperatorType op)
    {
      Condition.Requires(left).IsNotNull();
      Condition.Requires(right).IsNotNull();

      Left = left;
      Right = right;
      Operator = op;
    }


    public override object Evaluate(EvaluationContext context)
    {
      object a = Left.Evaluate(context);
      object b = Right.Evaluate(context);

      switch (Operator)
      {
        case OperatorType.Equals:
          return EqualsOp(a, b);
      }

      throw new InvalidOperationException("Unhandled operator type: " + Operator);
    }


    private bool EqualsOp(object a, object b)
    {
      if (a == null && b == null)
        return true;
      if (a == null || b == null)
        return false;

      return a.Equals(b);
    }
  }
}

