using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ReactionGeneratorStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      // FIXME: what about "executeScheduledRules"
      EvaluationContext context = new EvaluationContext(item.State, item.Input, item.Request.RuleId, executeScheduledRules: false);

      item.KnowledgeBase.FindMatchingReactions(context, item.Reactions);
    }
  }
}
