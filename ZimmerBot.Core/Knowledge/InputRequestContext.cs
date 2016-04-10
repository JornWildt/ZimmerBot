using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Knowledge
{
  public class InputRequestContext
  {
    public RequestContext RequestContext { get; protected set; }

    public Request Request { get; protected set; }

    public ZTokenSequence Input { get; set; }

    public bool FromTemplate { get; protected set; }

    public bool DoContinueMatchingRules { get; protected set; }

    public string InputForNextRuleMatching { get; protected set; }


    public InputRequestContext(RequestContext context, Request request, ZTokenSequence input)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();
      Condition.Requires(request, nameof(request)).IsNotNull();

      RequestContext = context;
      Request = request;
      Input = input;
    }


    public void Continue(string input = null)
    {
      DoContinueMatchingRules = true;
      InputForNextRuleMatching = input;
    }
  }
}
