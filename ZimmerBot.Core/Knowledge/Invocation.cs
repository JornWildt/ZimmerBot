using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using static ZimmerBot.Core.Knowledge.ProcessorRegistry;


namespace ZimmerBot.Core.Knowledge
{
  public class Invocation
  {
    protected ProcessorRegistration P { get; set; }

    protected string[] ParameterNames { get; set; }

    protected Dictionary<string, string> OutputTemplates { get; set; }


    public Invocation(ProcessorRegistration p, params string[] parameterNames)
    {
      Condition.Requires(parameterNames, "parameterNames").IsNotNull();

      P = p;
      ParameterNames = parameterNames;
      OutputTemplates = new Dictionary<string, string>();
    }


    public Invocation WithTemplate(string template)
    {
      OutputTemplates.Add("default", template);
      return this;
    }


    public Invocation WithTemplate(string name, string template)
    {
      OutputTemplates.Add(name, template);
      return this;
    }


    public void Verify()
    {
      foreach (string requiredTemplateName in P.RequiredOutputTemplateNames)
      {
        if (!OutputTemplates.ContainsKey(requiredTemplateName))
          Logger.Warn($"Required template '{requiredTemplateName}' is missing in invocation of function '{P.Name}'.");
      }
    }


    public string Run(ResponseContext rc)
    {
      List<object> inputs = new List<object>();
      foreach (string parameterName in ParameterNames)
      {
        object value = rc.Match.Matches[parameterName];
        inputs.Add(value);
      }

      ProcessorInput input = new ProcessorInput(rc, inputs, OutputTemplates);
      return P.Processor(input)(); // FIXME - one too many delegates
    }
  }
}
