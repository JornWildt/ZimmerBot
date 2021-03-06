﻿using ZimmerBot.Core.Knowledge;
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

      if (item.Context.Input != null)
      {
        foreach (var input in item.Context.Input)
        {
          for (int i = 0; i < input.Count; ++i)
          {
            string word = input[i].OriginalText;
            string checkedWord = SpellChecker.SpellCheck(word);
            BotUtility.EvaluationLogger.Debug($"Spell checking {word} => {checkedWord}");
            input[i] = input[i].CorrectWord(checkedWord);
          }
        }
      }
    }
  }
}
