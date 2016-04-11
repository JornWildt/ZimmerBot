using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.Statements
{
  public class ContinueStatement : Statement
  {
    public enum TargetEnum { None, Label, Input }

    public string Target { get; protected set; }

    public TargetEnum TargetType { get; protected set; }


    public ContinueStatement()
    {
      TargetType = TargetEnum.None;
    }


    public ContinueStatement(string target, TargetEnum targetType)
    {
      Condition.Requires(target, nameof(target)).IsNotNullOrEmpty();

      Target = target;
      TargetType = targetType;
    }


    public ContinueStatement(List<string> target, TargetEnum targetType)
    {
      Condition.Requires(target, nameof(target)).IsNotNull();

      Target = target.Aggregate((a,b) => a + " " + b);
      TargetType = targetType;
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      ResponseGenerationContext rc = context.ResponseContext;

      string target = Target;
      if (TargetType == TargetEnum.Label && target != null)
        target = "@" + target;

      context.Continue(target);
    }
  }
}
