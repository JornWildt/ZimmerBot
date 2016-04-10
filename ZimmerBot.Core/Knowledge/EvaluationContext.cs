using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class EvaluationContext
  {
    public RequestState State { get; protected set; }

    public Session Session { get; protected set; }

    public Request OriginalRequest { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public string RestrictToRuleId { get; protected set; }

    public bool ExecuteScheduledRules { get; protected set; }

    public int CurrentTokenIndex { get; set; }

    public int CurrentRepetitionIndex { get; set; }

    public bool DoContinueMatchingRules { get; protected set; }

    public string InputForNextRuleMatching { get; protected set; }


    public EvaluationContext(RequestState state, Session session, Request originalRequest, ZTokenSequence input, string ruleId, bool executeScheduledRules)
    {
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(session, nameof(session)).IsNotNull();
      Condition.Requires(originalRequest, nameof(originalRequest)).IsNotNull();

      State = state;
      Session = session;
      OriginalRequest = originalRequest;
      Input = input;
      RestrictToRuleId = ruleId;
      ExecuteScheduledRules = executeScheduledRules;
    }


    public void Continue(string input = null)
    {
      DoContinueMatchingRules = true;
      InputForNextRuleMatching = input;
    }
  }
}
