using System.Collections;
using System.Collections.Generic;
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

    public Stack<string> MatchNames { get; protected set; }


    // FIXME: ruleId?
    public TriggerEvaluationContext(InputRequestContext inputContext, bool executeScheduledRules)
    {
      Condition.Requires(inputContext, nameof(inputContext)).IsNotNull();

      InputContext = inputContext;
      ExecuteScheduledRules = executeScheduledRules;
      CurrentRepetitionIndex = 1;
      MatchNames = new Stack<string>();
    }
  }
}
