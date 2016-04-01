using System;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class WordTaggingStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (item.Input != null)
        item.KnowledgeBase.ExpandTokens(item.Input);
    }
  }
}
