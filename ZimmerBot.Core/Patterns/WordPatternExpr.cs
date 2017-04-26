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


    public override string ToString()
    {
      return Word;
    }


    public override void ExtractWordsForSpellChecker()
    {
      SpellChecker.AddWord(Word);
    }


    public override void ExtractMatchValues(Dictionary<string, string> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
    }
  }
}
