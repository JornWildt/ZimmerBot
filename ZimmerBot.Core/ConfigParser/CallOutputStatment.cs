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
      ExpressionEvaluationContext ec = new ExpressionEvaluationContext(context.ResponseContext.Variables);

      List<object> inputs = Function.CalculateInputValues(ec);

      ProcessorInput inp = new ProcessorInput(context.ResponseContext, inputs);
      ProcessorOutput result = processor.Function(inp);

      context.LastValue = result;

      // Make the output values available to templates
      if (result.Value is IDictionary<string, object>)
      {
        context.ResponseContext.Variables.Push((IDictionary < string, object > )result.Value);
      }
    }
  }
}
