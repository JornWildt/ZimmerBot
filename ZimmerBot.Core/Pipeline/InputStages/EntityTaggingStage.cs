using System.Linq;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  // A simple entity tagging processor that tries to match multiple words with registered entity names
  public class EntityTaggingStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (item.Context.Input == null)
        return;

      ZTokenSequence input = item.Context.Input;

      ZTokenSequence output = item.Context.KnowledgeBase.EntityManager.CalculateLabels(input);

      input.Clear();
      input.AddRange(output);
    }
  }
}
