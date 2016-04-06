using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Expressions
{
  public class UnaryOperatorExpr : Expression
  {
    public enum OperatorType { Negation }

    public Expression Expr { get; protected set; }

    public OperatorType Operator { get; protected set; }


    public UnaryOperatorExpr(Expression expr, OperatorType op)
    {
      Condition.Requires(expr).IsNotNull();

      Expr = expr;
      Operator = op;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      object a = Expr.Evaluate(context);

      switch (Operator)
      {
        case OperatorType.Negation:
          return NegationOp(a);
      }

      throw new InvalidOperationException("Unhandled operator type: " + Operator);
    }


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      throw new ApplicationException("It is not possible to assign to a unary expression.");
    }


    public override string ToString()
    {
      string op = "???";
      switch (Operator)
      {
        case OperatorType.Negation:
          op = "!";
          break;
      }
      return op + Expr.ToString();
    }


    private bool NegationOp(object a)
    {
      bool b = ConvertToBool(a);
      return !b;
    }
  }
}
