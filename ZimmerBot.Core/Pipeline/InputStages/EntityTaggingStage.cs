using System.Collections.Generic;
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

      // Take a copy of the input list since we are adding to it in the loop
      foreach (ZTokenSequence input in item.Context.Input.ToArray())
      {
        item.Context.KnowledgeBase.EntityManager.FindEntities(input, item.Context.Input);
      }
    }
  }
}
