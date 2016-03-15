using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Expressions
{
  public class IdentifierExpression : Expression
  {
    public string Identifier { get; protected set; }


    public IdentifierExpression(string identifier)
    {
      Condition.Requires(identifier, "identifier").IsNotNullOrEmpty();

      Identifier = identifier;
    }


    public override object Evaluate(EvaluationContext context)
    {
      return context.State[Identifier];
    }
  }
}
