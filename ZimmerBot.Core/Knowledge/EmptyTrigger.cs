using Quartz;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class EmptyTrigger : Trigger
  {
    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId)
    {
      // Do nothing
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (context.InputContext.Input != null)
        return new MatchResult(0);
      else
        return new MatchResult(1);
    }
  }
}
