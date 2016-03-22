using System.Collections.Generic;
using CuttingEdge.Conditions;
using log4net;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


// OBSOLETE

namespace ZimmerBot.Core.Processors
{
  public class CallBinding
  {
    private static ILog Logger = LogManager.GetLogger(typeof(CallBinding));

    public ProcessorRegistration Processor { get; protected set; }

    public string[] ParameterNames { get; protected set; }

    public Dictionary<string, string> OutputTemplates { get; protected set; }


    public CallBinding(ProcessorRegistration processor, params string[] parameterNames)
    {
      Condition.Requires(processor, nameof(processor)).IsNotNull();
      Condition.Requires(parameterNames, nameof(parameterNames)).IsNotNull();

      Processor = processor;
      ParameterNames = parameterNames;
      OutputTemplates = new Dictionary<string, string>();
    }


    public CallBinding WithTemplate(string template)
    {
      OutputTemplates.Add("default", template);
      return this;
    }


    public CallBinding WithTemplate(string name, string template)
    {
      OutputTemplates.Add(name, template);
      return this;
    }


    public void VerifyBinding()
    {
      foreach (string requiredTemplateName in Processor.RequiredOutputTemplateNames)
      {
        if (!OutputTemplates.ContainsKey(requiredTemplateName))
          Logger.Warn($"Required template '{requiredTemplateName}' is missing in binding of function '{Processor.Name}'.");
      }
    }


    public object Invoke(ResponseContext context)
    {
      List<object> inputs = new List<object>();
      foreach (string parameterName in ParameterNames)
      {
        object value = context.Match.Matches[parameterName];
        inputs.Add(value);
      }

      ProcessorInput input = new ProcessorInput(context, inputs);
      return Processor.Function(input);
    }
  }
}
