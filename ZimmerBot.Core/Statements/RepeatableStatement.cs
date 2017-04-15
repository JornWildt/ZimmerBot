  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Statements
{
  public class RepeatableStatement : Statement
  {
    public bool IsRepeatable { get; protected set; }

    public override RepatableMode Repeatable { get { return IsRepeatable ? RepatableMode.ForcedRepeatable : RepatableMode.ForcedSingle; } }


    public RepeatableStatement(bool isRepeatable)
    {
      IsRepeatable = isRepeatable;
    }


    public override void Execute(StatementExecutionContect context)
    {
    }


    public override void Initialize(StatementInitializationContext context)
    {
    }
  }
}
