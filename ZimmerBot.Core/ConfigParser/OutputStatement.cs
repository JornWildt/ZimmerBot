using System.Collections.Generic;
using System.Text;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;

namespace ZimmerBot.Core.ConfigParser
{
  public abstract class OutputStatement
  {
    public abstract void Execute(OutputExecutionContect context);
  }


  public class OutputExecutionContect
  {
    public ResponseContext ResponseContext { get; set; }
    public StringBuilder Result { get; set; }
    public List<string> OutputTemplates { get; set; }
    public object LastValue { get; set; }

    public OutputExecutionContect(ResponseContext rcontext)
    {
      Condition.Requires(rcontext, nameof(rcontext)).IsNotNull();
      Result = new StringBuilder();
      OutputTemplates = new List<string>();
      LastValue = new object();
    }
  }


  public class TemplateOutputStatement : OutputStatement
  {
    public string Template { get; set; }

    public TemplateOutputStatement(string template)
    {
      Condition.Requires(template, nameof(template)).IsNotNull();
      Template = template;
    }

    public override void Execute(OutputExecutionContect context)
    {
      context.OutputTemplates.Add(Template);
    }
  }


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

      ProcessorInput inp = new ProcessorInput(context.ResponseContext, new List<object>());
      context.LastValue = processor.Function(inp);
    }
  }
}
