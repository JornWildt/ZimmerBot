using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class ConceptPatternExpr : PatternExpr
  {
    public string Word { get; protected set; }


    public ConceptPatternExpr(string cword)
    {
      Condition.Requires(cword, nameof(cword)).IsNotNullOrWhiteSpace();

      if (cword.StartsWith("%"))
        cword = cword.Substring(1);

      Word = cword;
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
      return "%" + Word;
    }


    public override void ExtractMatchValues(Dictionary<string, string> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
    }
  }
}
