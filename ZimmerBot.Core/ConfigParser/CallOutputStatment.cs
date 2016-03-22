using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.ConfigParser
{
  public class CallOutputStatment : OutputStatement
  {
    public FunctionCallExpr Function { get; set; }


    public CallOutputStatment(FunctionCallExpr function)
    {
      Condition.Requires(function, nameof(function)).IsNotNull();
      Function = function;
    }


    public override void Execute(OutputExecutionContect context)
    {
      ProcessorRegistration processor = ProcessorRegistry.GetProcessor(Function.FunctionName);
      ExpressionEvaluationContext ec = new ExpressionEvaluationContext();

      // FIXME: do some more efficient combination of matches, botstate and other state elements for lookup
      foreach (var m in context.ResponseContext.Match.Matches)
      {
        ec.State["$" + m.Key] = m.Value;
      }

      List<object> inputs = new List<object>();
      foreach (Expression expr in Function.Parameters)
      {
        //context.ResponseContext.State
        object p = expr.Evaluate(ec);
        inputs.Add(p);
      }

      ProcessorInput inp = new ProcessorInput(context.ResponseContext, inputs);
      context.LastValue = processor.Function(inp);
    }
  }
}
