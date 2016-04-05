namespace ZimmerBot.Core.Expressions
{
  public abstract class Expression
  {
    public abstract object Evaluate(ExpressionEvaluationContext context);
    public abstract void AssignValue(ExpressionEvaluationContext context, object value);
  }
}
