using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ReactionGeneratorStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      item.Context.KnowledgeBase.FindMatchingReactions(item.EvaluationContext, item.Reactions);
    }
  }
}
