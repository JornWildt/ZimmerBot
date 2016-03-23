using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Expressions
{
  public class ExpressionEvaluationContext
  {
    public IDictionary<string, object> Variables { get; protected set; }


    public ExpressionEvaluationContext(IDictionary<string, object> variables)
    {
      Condition.Requires(variables, nameof(variables)).IsNotNull();
      Variables = variables;
    }
  }
}
