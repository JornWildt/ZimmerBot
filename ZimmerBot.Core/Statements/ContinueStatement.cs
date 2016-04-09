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
    public Expression Next { get; protected set; }


    public ContinueStatement()
    {
    }


    public ContinueStatement(Expression next)
    {
      Next = next;
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      ResponseContext rc = context.ResponseContext;

      string nextInput = null;
      if (Next != null)
      {
        ExpressionEvaluationContext ec = rc.BuildExpressionEvaluationContext();
        nextInput = Next.Evaluate(ec) as string;
      }

      Request request = new Request(context.ResponseContext.OriginalRequest, nextInput);
      Response response = BotUtility.InvokeInternal(context.ResponseContext.KnowledgeBase, request, false, true);

      context.AdditionalOutput.AddRange(response.Output);
    }
  }
}
