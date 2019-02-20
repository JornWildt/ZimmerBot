using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.Knowledge
{
  public class ConditionRuleModifier : RuleModifier
  {
    Expression Expr;

    public ConditionRuleModifier(Expression expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      Expr = expr;
    }

    public override void Invoke(Executable e)
    {
      e.WithCondition(Expr);
    }
  }
}
