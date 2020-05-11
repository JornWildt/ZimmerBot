using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Patterns
{
  public class NegationPatternExpr : PatternExpr
  {
    public string Word { get; protected set; }


    public NegationPatternExpr(string word)
    {
      Condition.Requires(word, nameof(word)).IsNotNull();
      Word = word;
    }


    public override string Identifier => Word;


    public override double Weight
    {
      get { return 0; }
    }


    public override double ProbabilityFactor
    {
      get { return 2.0; }
    }


    public override void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
    }


    public override void ExtractWordsForSpellChecker()
    {
      SpellChecker.AddWord(Word);
    }


    public override string ToString()
    {
      return "~" + Word;
    }

    public override double CalculateMatch(ZTokenSequence input, int myPos, List<PatternExpr> expressions)
    {
      for (int i = 0; i < input.Count; ++i)
      {
        if (input[i].Type == ZToken.TokenType.Word && input[i].Matches(Word))
        {
          return -input.Count;
        }
      }

      // No match found - that is good
      return 1;
    }
  }
}
