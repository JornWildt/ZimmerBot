using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Patterns
{
  public class WordPatternExpr : PatternExpr
  {
    public string Word { get; protected set; }


    public WordPatternExpr(string word)
    {
      Condition.Requires(word, nameof(word)).IsNotNullOrWhiteSpace();

      Word = word;
    }


    public override string Identifier
    {
      get { return Word; }
    }


    public override double Weight
    {
      get { return 1; }
    }


    public override double ProbabilityFactor
    {
      get { return 1.0; }
    }


    public override string ToString()
    {
      return Word;
    }


    public override void ExtractWordsForSpellChecker()
    {
      SpellChecker.AddWord(Word);
    }


    public override void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
    }


    public override double CalculateMatch(ZTokenSequence input, int myPos, List<PatternExpr> expressions)
    {
      for (int i = 0; i < input.Count; ++i)
      {
        if (input[i].Type == ZToken.TokenType.Word && input[i].Matches(Word))
        {
          double maxSize = Math.Max(expressions.Count, input.Count);
          int dist = i - myPos;
          return (double)(maxSize - Math.Abs(dist)) / maxSize;
        }
      }

      // No match found - make sure total score is reduced (avoids input "a" matching pattern "a b" perfectly).
      return -0.5;
    }
  }
}
