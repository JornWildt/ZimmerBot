using System.Linq;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class SentenceTaggingStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (item.Context.Input == null)
        return;
    }
  }
}
