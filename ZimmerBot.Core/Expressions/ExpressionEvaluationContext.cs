using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Expressions
{
  public class ExpressionEvaluationContext
  {
    public IDictionary<string, object> Variables { get; protected set; }

    public Session Session { get; protected set; }


    public ExpressionEvaluationContext(Session session, IDictionary<string, object> variables)
    {
      Condition.Requires(session, nameof(session)).IsNotNull();
      Condition.Requires(variables, nameof(variables)).IsNotNull();

      Session = session;
      Variables = variables;
    }
  }
}
