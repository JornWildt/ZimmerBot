using CuttingEdge.Conditions;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class WildcardPatternExpr : PatternExpr
  {
    public string Name { get; protected set; }

    public override string Identifier => Name;

    public override double Weight => 1.0;

    public override double ProbabilityFactor => 1.0;

    public WildcardPatternExpr(string name)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
      Name = name;
    }


    public override string ToString()
    {
      return Name;
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
      // FIXME: do something?
    }


    public override double CalculateMatch(ZTokenSequence input, int i, List<PatternExpr> expressions)
    {
      return 0.0;
    }
  }
}
