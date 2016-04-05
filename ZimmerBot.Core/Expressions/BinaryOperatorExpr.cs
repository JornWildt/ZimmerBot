using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class BinaryOperatorExpr : Expression
  {
    public enum OperatorType { Equals, NotEquals }

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


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      object a = Left.Evaluate(context);
      object b = Right.Evaluate(context);

      switch (Operator)
      {
        case OperatorType.Equals:
          return EqualsOp(a, b);
        case OperatorType.NotEquals:
          return !EqualsOp(a, b);
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


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      throw new ApplicationException("It is not possible to assign to a binary expression.");
    }


    public override string ToString()
    {
      string op = "???";
      switch (Operator)
      {
        case OperatorType.Equals:
          op = "=";
          break;
        case OperatorType.NotEquals:
          op = "!=";
          break;
      }
      return Left.ToString() + op + Right.ToString();
    }
  }
}
