using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.Statements
{
  public class SetStatement : Statement
  {
    public Expression Reference { get; protected set; }

    public Expression ValueExpr { get; protected set; }


    public SetStatement(Expression reference, Expression expr)
    {
      Condition.Requires(reference, nameof(reference)).IsNotNull().IsOfType(typeof(DotExpression));
      Condition.Requires(expr, nameof(expr)).IsNotNull();

      Reference = reference;
      ValueExpr = expr;
    }


    public override RepatableMode Repeatable
    {
      get { return RepatableMode.Undefined; }
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      ExpressionEvaluationContext ec = new ExpressionEvaluationContext(context.ResponseContext.Variables);
      object value = ValueExpr.Evaluate(ec);
      Reference.AssignValue(ec, value);
    }
  }
}
