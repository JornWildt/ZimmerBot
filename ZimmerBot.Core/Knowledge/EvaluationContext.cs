using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class EvaluationContext
  {
    public BotState State { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public string RestrictToRuleId { get; protected set; }

    public bool ExecuteScheduledRules { get; protected set; }

    public int CurrentTokenIndex { get; set; }


    public EvaluationContext(BotState state, ZTokenSequence input, string ruleId, bool executeScheduledRules)
    {
      Condition.Requires(state, "state").IsNotNull();

      State = state;
      Input = input;
      RestrictToRuleId = ruleId;
      ExecuteScheduledRules = executeScheduledRules;
    }
  }
}
