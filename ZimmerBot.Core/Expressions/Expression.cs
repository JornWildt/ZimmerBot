using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Expressions
{
  public abstract class Expression
  {
    public abstract object Evaluate(EvaluationContext context);
  }
}
