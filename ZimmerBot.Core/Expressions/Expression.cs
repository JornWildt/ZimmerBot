using System;

namespace ZimmerBot.Core.Expressions
{
  public abstract class Expression
  {
    public abstract object Evaluate(ExpressionEvaluationContext context);
    public abstract void AssignValue(ExpressionEvaluationContext context, object value);


    public static bool ConvertToBool(object a)
    {
      bool b;
      if (!TryConvertToBool(a, out b))
        throw new InvalidCastException($"Cannot convert value '{a}' to boolean");
      return b;
    }


    public static bool TryConvertToBool(object a, out bool b)
    {
      b = false;

      if (a is bool)
        b = (bool)a;
      else if (a is int)
        b = (int)a != 0;
      else if (a is double)
        b = (int)a != 0d;
      else if (a is string)
        b = !string.IsNullOrEmpty((string)a);
      else if (a == null)
        b = false;
      else
        return false;

      return true;
    }
  }
}
