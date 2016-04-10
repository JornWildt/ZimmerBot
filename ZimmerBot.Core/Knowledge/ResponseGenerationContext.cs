using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ResponseGenerationContext
  {
    public InputRequestContext InputContext { get; protected set; }

    public WRegex.MatchResult Match { get; protected set; }


    public ResponseGenerationContext(InputRequestContext context, WRegex.MatchResult match)
    {
      // match can be null for scheduled, non-input based, responses
      Condition.Requires(context, nameof(context)).IsNotNull();

      InputContext = context;
      Match = match;
    }


    public ExpressionEvaluationContext BuildExpressionEvaluationContext()
    {
      return new ExpressionEvaluationContext(InputContext.RequestContext.Variables);
    }


    public void Continue(string input = null)
    {
      InputContext.Continue(input);
    }
  }
}
