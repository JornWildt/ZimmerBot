using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class ConstantNumberExpr : Expression
  {
    public double Value { get; protected set; }


    public ConstantNumberExpr(double value)
    {
      Value = value;
    }


    public override object Evaluate(EvaluationContext context)
    {
      return Value;
    }
  }
}
