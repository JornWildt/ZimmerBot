using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class TopicAssigningStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      // FIXME Unused
      //item.Context.KnowledgeBase.FindCurrentTopic(item.EvaluationContext);
    }
  }
}
