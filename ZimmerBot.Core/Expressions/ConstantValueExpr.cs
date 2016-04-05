using System;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class ConstantValueExpr : Expression
  {
    public object Value { get; protected set; }


    public ConstantValueExpr(object value)
    {
      Value = value;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      return Value;
    }


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      throw new ApplicationException("It is not possible to assign to constant value.");
    }


    public override string ToString()
    {
      return Value.ToString();
    }
  }
}
