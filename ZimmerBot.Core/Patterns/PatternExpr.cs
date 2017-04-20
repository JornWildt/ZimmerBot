using System.Collections.Generic;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public abstract class PatternExpr
  {
    public abstract string Identifier { get; }

    public abstract double Weight { get; }

    public abstract void ExtractMatchValues(Dictionary<string, string> matchValues, Queue<ZToken> entityTokens);
  }
}
