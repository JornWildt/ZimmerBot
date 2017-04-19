using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Patterns
{
  public class Pattern
  {
    public List<PatternExpr> Expressions { get; protected set; }

    public Pattern(List<PatternExpr> expressions)
    {
      Condition.Requires(expressions, nameof(expressions)).IsNotNull();
      Expressions = expressions;
    }
  }
}
