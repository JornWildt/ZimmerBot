using System.Linq;
using Quartz;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class EmptyTrigger : Trigger
  {
    public override string ToString()
    {
      return "<empty>";
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (context.InputContext.Input == null || context.InputContext.Input.All(inp => inp.Count == 0))
      {
        double conditionModifier = CalculateConditionModifier(context);
        return new MatchResult(conditionModifier);
      }
      else
        return new MatchResult(0);
    }
  }
}
