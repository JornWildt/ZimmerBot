using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public abstract class PatternExpr
  {
    public abstract string Identifier { get; }

    public abstract double Weight { get; }

    public abstract double ProbabilityFactor { get; }

    public abstract void ExtractWordsForSpellChecker();

    public abstract void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens);
  }
}
