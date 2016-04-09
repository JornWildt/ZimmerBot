using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ReactionGeneratorStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      // FIXME: what about "executeScheduledRules"
      item.KnowledgeBase.FindMatchingReactions(item.EvaluationContext, item.Reactions);
    }
  }
}
