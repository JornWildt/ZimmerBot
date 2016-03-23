using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.ConfigParser
{
  public class OutputExecutionContect
  {
    public ResponseContext ResponseContext { get; set; }

    public Dictionary<string,List<string>> OutputTemplates { get; set; }

    public ProcessorOutput LastValue { get; set; }


    public OutputExecutionContect(ResponseContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      ResponseContext = context;
      OutputTemplates = new Dictionary<string, List<string>>();
      LastValue = null;
    }


    public void AddOutputTemplate(KeyValuePair<string, string> template)
    {
      if (!OutputTemplates.ContainsKey(template.Key))
        OutputTemplates[template.Key] = new List<string>();
      OutputTemplates[template.Key].Add(template.Value);
    }
  }
}
