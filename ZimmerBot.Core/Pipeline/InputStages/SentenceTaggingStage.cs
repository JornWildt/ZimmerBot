using System.Linq;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class SentenceTaggingStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (item.Input == null)
        return;

      if (item.Input.Any(inp => inp.OriginalText == "hej"))
        item.SentenceTags.Add("hilsen");
    }
  }
}
