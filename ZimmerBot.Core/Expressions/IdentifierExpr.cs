using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class IdentifierExpr : Expression
  {
    public string Identifier { get; protected set; }


    public IdentifierExpr(string identifier)
    {
      Condition.Requires(identifier, "identifier").IsNotNullOrEmpty();

      Identifier = identifier;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      return context.Variables[Identifier];
    }


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      throw new NotImplementedException(); // FIXME
    }
  }
}
