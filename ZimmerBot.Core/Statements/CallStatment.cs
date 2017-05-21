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


    public override RepatableMode Repeatable
    {
      get { return RepatableMode.AutomaticRepeatable; }
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      ProcessorRegistration processor = ProcessorRegistry.GetProcessor(Function.FunctionName);
      ExpressionEvaluationContext ec = context.ResponseContext.BuildExpressionEvaluationContext();

      ec.Variables["$_"] = context.LastValue;

      List<object> inputs = Function.CalculateInputValues(ec);

      ProcessorInput inp = new ProcessorInput(context.ResponseContext, inputs);
      ProcessorOutput result = processor.Function(inp);

      context.LastValue = result;

      // Make the output values available to templates
      if (result.Value != null)
      {
        // Remove optional existing "last value" dictionary (and all other stuff "on top" of it)
        while (context.ResponseContext.Variables.ContainsKey("####RESULT####"))
          context.ResponseContext.Variables.Pop();

        // Mark this dictionary as the special "last value" dictionary
        result.Value["####RESULT####"] = true;

        context.ResponseContext.Variables.Push(result.Value);
      }
    }
  }
}
