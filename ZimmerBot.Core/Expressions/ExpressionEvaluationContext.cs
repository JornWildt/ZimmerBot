using System.Collections.Generic;


namespace ZimmerBot.Core.Expressions
{
  public class ExpressionEvaluationContext
  {
    public Dictionary<string, object> State { get; protected set; }

    public ExpressionEvaluationContext()
    {
      State = new Dictionary<string, object>();
    }
  }
}
