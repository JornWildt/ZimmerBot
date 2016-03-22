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
  }
}
