using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class NegationPatternExpr : PatternExpr
  {
    public PatternExpr SubExpr { get; protected set; }


    public NegationPatternExpr(PatternExpr expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      SubExpr = expr;
    }


    public override string Identifier
    {
      get
      {
        return SubExpr.ToString();
      }
    }


    public override double Weight
    {
      get { return -5; }
    }


    public override void ExtractMatchValues(Dictionary<string, string> matchValues, Queue<ZToken> entityTokens)
    {
      // Do nothing
    }


    public override void ExtractWordsForSpellChecker()
    {
      SubExpr.ExtractWordsForSpellChecker();
    }


    public override string ToString()
    {
      return "~" + SubExpr.ToString();
    }
  }
}
