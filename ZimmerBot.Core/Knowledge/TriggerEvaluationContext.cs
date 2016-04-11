using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class TriggerEvaluationContext
  {
    public InputRequestContext InputContext { get; protected set; }

    public string RestrictToRuleId { get { return InputContext.Request.RuleId; } }

    public string RestrictToRuleLabel { get { return InputContext.Request.RuleLabel; } }

    public bool ExecuteScheduledRules { get; protected set; }

    public int CurrentTokenIndex { get; set; }

    public int CurrentRepetitionIndex { get; set; }


    // FIXME: ruleId?
    public TriggerEvaluationContext(InputRequestContext inputContext, bool executeScheduledRules)
    {
      Condition.Requires(inputContext, nameof(inputContext)).IsNotNull();

      InputContext = inputContext;
      ExecuteScheduledRules = executeScheduledRules;
    }
  }
}
