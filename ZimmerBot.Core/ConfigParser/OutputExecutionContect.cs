using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.ConfigParser
{
  public class OutputExecutionContect
  {
    public ResponseContext ResponseContext { get; set; }

    public List<string> OutputTemplates { get; set; }

    public object LastValue { get; set; }


    public OutputExecutionContect(ResponseContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      ResponseContext = context;
      OutputTemplates = new List<string>();
      LastValue = new object();
    }
  }
}
