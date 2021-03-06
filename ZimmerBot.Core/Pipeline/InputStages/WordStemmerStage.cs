﻿using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class WordStemmerStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (!AppSettings.EnableStemming)
        return;

      if (item.Context.Input == null)
        return;

      foreach (ZTokenSequence input in item.Context.Input)
      {
        if (input != null)
        {
          for (int i = 0; i < input.Count; ++i)
          {
            string word = input[i].OriginalText;
            string stem = SpellChecker.Stem(word);
            BotUtility.EvaluationLogger.Debug($"Stemming {word} => {stem}");
            input[i] = input[i].CorrectWord(stem);
          }
        }
      }
    }
  }
}
