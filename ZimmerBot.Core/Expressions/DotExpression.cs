using System;
using System.Linq;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using System.Collections;

namespace ZimmerBot.Core.Expressions
{
  public class DotExpression : Expression
  {
    public Expression Left { get; protected set; }

    public string Right { get; protected set; }


    public DotExpression(Expression left, string right)
    {
      Condition.Requires(left, nameof(left)).IsNotNull();
      Condition.Requires(right, nameof(right)).IsNotNullOrEmpty();

      Left = left;
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
        if (value is IDictionary<string, object>)
        {
          left = (IDictionary<string, object>)value;
        }
        else if (value is System.Collections.IDictionary)
        {
          var dict = (System.Collections.IDictionary)value;
          left = new Dictionary<string, object>();
          foreach (object key in dict.Keys)
            left[key.ToString()] = dict[key];
        }
        else if (value == null)
          throw new InvalidOperationException($"Could not evaluate '{Right}' from null value in dot-operator.");
        else
        throw new InvalidOperationException($"Could not evaluate '{Right}' from '{value?.GetType()}' in dot-operator - expected dictionary<string,object>.");
      }

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