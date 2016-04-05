using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Expressions
{
  public class DotExpression : Expression
  {
    public DotExpression Left { get; protected set; }

    public string Right { get; protected set; }


    public DotExpression(string right)
    {
      Condition.Requires(right, nameof(right)).IsNotNullOrEmpty();

      Right = right;
    }


    public DotExpression(Expression left, string right)
    {
      Condition.Requires(left, nameof(left)).IsNotNull().IsOfType(typeof(DotExpression));
      Condition.Requires(right, nameof(right)).IsNotNullOrEmpty();

      Left = (DotExpression)left;
      Right = right;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      IDictionary<string, object> left;

      if (Left == null)
      {
        left = context.Variables;
      }
      else
      {
        object value = Left.Evaluate(context);
        if (!(value is IDictionary<string, object>))
          throw new InvalidOperationException($"Could not evaluate '{Right}' from '{value?.GetType()}' - expected dictionary.");
        left = (IDictionary<string, object>)value;
      }

      if (!left.ContainsKey(Right))
        return null;
      return left[Right];
    }


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      if (Left == null)
        throw new InvalidOperationException("Unexpected assignment to dot-expression with no left-value");

      object leftValue = Left.Evaluate(context);

      if (!(leftValue is IDictionary<string, object>))
        throw new InvalidOperationException($"Could not evaluate '{Right}' from '{leftValue?.GetType()}' in assignment - expected dictionary.");
      IDictionary<string, object> left = (IDictionary<string, object>)leftValue;

      left[Right] = value;
    }


    public override string ToString()
    {
      if (Left != null)
        return Left.ToString() + "." + Right;
      else
        return Right;
    }
  }
}