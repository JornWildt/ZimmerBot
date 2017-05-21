using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Processors
{
  public class ProcessorOutput
  {
    public string TemplateName { get; protected set; }

    public IDictionary<string, object> Value { get; protected set; }


    public ProcessorOutput(IDictionary<string, object> value)
      : this("default", value)
    {
    }


    public ProcessorOutput(string templateName, IDictionary<string, object> value)
    {
      Condition.Requires(templateName, nameof(templateName)).IsNotNullOrEmpty();
      TemplateName = templateName;
      Value = value;
    }
  }
}
