using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.ConfigParser
{
  public class SetOutputStatement : OutputStatement
  {
    public Expression Reference { get; protected set; }

    public Expression ValueExpr { get; protected set; }


    public SetOutputStatement(Expression reference, Expression expr)
    {
      Condition.Requires(reference, nameof(reference)).IsNotNull().IsOfType(typeof(DotExpression));
      Condition.Requires(expr, nameof(expr)).IsNotNull();

      Reference = reference;
      ValueExpr = expr;
    }


    public override void Initialize(OutputInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(OutputExecutionContect context)
    {
      ExpressionEvaluationContext ec = new ExpressionEvaluationContext(context.ResponseContext.Variables);
      object value = ValueExpr.Evaluate(ec);
      Reference.AssignValue(ec, value);
    }
  }
}
