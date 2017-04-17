using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class SpellCheckerStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (!AppSettings.EnableSpellingCorrections)
        return;

      ZTokenSequence input = item.Context.Input;

      if (input != null)
      {
        for (int i = 0; i < input.Count; ++i)
        {
          string word = input[i].OriginalText;
          string checkedWord = SpellChecker.SpellCheck(word);
          BotUtility.EvaluationLogger.Debug($"Spell checking {word} => {checkedWord}");
          input[i] = new ZToken(checkedWord, input[i]);
        }
      }
    }
  }
}
