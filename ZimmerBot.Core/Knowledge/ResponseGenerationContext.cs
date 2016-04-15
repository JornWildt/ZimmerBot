using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ResponseGenerationContext
  {
    public InputRequestContext InputContext { get; protected set; }

    public KnowledgeBase KnowledgeBase { get { return InputContext.KnowledgeBase; } }

    public ChainedDictionary<string, object> Variables { get { return InputContext.Variables; } }

    public RequestState State { get { return InputContext.State; } }

    public Session Session { get { return InputContext.Session; } }

    public Request Request { get { return InputContext.Request; } }

    public int RepetitionCount
    {
      get
      {
        return InputContext.RepetitionCount;
      }
      set
      {
        InputContext.RepetitionCount = value;
      }
    }


    public MatchResult Match { get; protected set; }


    public ResponseGenerationContext(InputRequestContext context, MatchResult match)
    {
      // match can be null for scheduled, non-input based, responses
      Condition.Requires(context, nameof(context)).IsNotNull();

      InputContext = context;
      Match = match;
    }


    public ExpressionEvaluationContext BuildExpressionEvaluationContext()
    {
      return new ExpressionEvaluationContext(InputContext.Variables);
    }


    public void Continue(string input = null)
    {
      InputContext.Continue(input);
    }
  }
}
