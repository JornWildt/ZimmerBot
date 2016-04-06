using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class EvaluationContext
  {
    public RequestState State { get; protected set; }

    public Request OriginalRequest { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public string RestrictToRuleId { get; protected set; }

    public bool ExecuteScheduledRules { get; protected set; }

    public int CurrentTokenIndex { get; set; }

    public int CurrentRepetitionIndex { get; set; }


    public EvaluationContext(RequestState state, Request originalRequest, ZTokenSequence input, string ruleId, bool executeScheduledRules)
    {
      Condition.Requires(state, "state").IsNotNull();
      Condition.Requires(originalRequest, nameof(originalRequest)).IsNotNull();

      State = state;
      OriginalRequest = originalRequest;
      Input = input;
      RestrictToRuleId = ruleId;
      ExecuteScheduledRules = executeScheduledRules;
    }
  }
}
