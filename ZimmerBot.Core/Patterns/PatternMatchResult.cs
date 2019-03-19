using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class PatternMatchResult
  {
    public Pattern MatchPattern { get; protected set; }

    public Dictionary<string, ZToken> MatchValues { get; protected set; }


    public PatternMatchResult(Pattern pattern, ZTokenSequence input, IEnumerable<KeyValuePair<string,string>> identifiers)
    {
      Condition.Requires(pattern, nameof(pattern)).IsNotNull();

      MatchPattern = pattern;
      MatchValues = new Dictionary<string, ZToken>(StringComparer.OrdinalIgnoreCase);

      foreach (var id in identifiers)
      {
        if (id.Value != null)
          MatchValues.Add(id.Key, new ZToken(id.Value));
        else
          MatchValues.Add(id.Key, null);
      }

      Queue<ZToken> entityTokens = new Queue<ZToken>(input.Where(t => t.Type == ZToken.TokenType.Entity));
      foreach (PatternExpr expr in MatchPattern.Expressions)
      {
        expr.ExtractMatchValues(MatchValues, entityTokens);
      }
    }


    public bool HasPatternWithParameter(string p)
    {
      return MatchPattern.HasParameterNamed(p);
    }


    public override string ToString()
    {
      string values = MatchValues.Select(v => v.Key + ":" + v.Value).Aggregate((a, b) => a + ", " + b);
      return MatchPattern.ToString() + " => " + values;
    }
  }
}
