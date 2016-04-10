using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Statements
{
  public class CallStatment : Statement
  {
    public FunctionCallExpr Function { get; set; }


    public CallStatment(FunctionCallExpr function)
    {
      Condition.Requires(function, nameof(function)).IsNotNull();
      Function = function;
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      ProcessorRegistration processor = ProcessorRegistry.GetProcessor(Function.FunctionName);
      ExpressionEvaluationContext ec = context.ResponseContext.BuildExpressionEvaluationContext();

      List<object> inputs = Function.CalculateInputValues(ec);

      ProcessorInput inp = new ProcessorInput(context.ResponseContext, inputs);
      ProcessorOutput result = processor.Function(inp);

      context.LastValue = result;

      // Make the output values available to templates
      if (result.Value is IDictionary<string, object>)
      {
        context.ResponseContext.InputContext.RequestContext.Variables.Push((IDictionary < string, object > )result.Value);
      }
    }
  }
}
