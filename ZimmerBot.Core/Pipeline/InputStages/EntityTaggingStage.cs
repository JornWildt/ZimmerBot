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

      for (int i = 0; i < input.Count; ++i)
      {
        for (int j = input.Count-1; j >= i; --j)
        {
          string inputWord = input[i].OriginalText;
          for (int k = i + 1; k <= j; ++k)
            inputWord += " " + input[k].OriginalText;

          if (item.Context.KnowledgeBase.Entities.ContainsKey(inputWord))
          {
            input.RemoveRange(i + 1, j - i);
            input[i] = new ZToken(item.Context.KnowledgeBase.Entities[inputWord].OriginalLabel, ZToken.TokenType.Entity);
            ++i;
          }

          if (i >= input.Count)
            break;
        }
      }
    }
  }
}
