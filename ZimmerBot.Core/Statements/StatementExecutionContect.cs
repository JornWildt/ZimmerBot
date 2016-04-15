using System.Collections.Generic;
using System.Text;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Statements
{
  public class StatementExecutionContect
  {
    public ResponseGenerationContext ResponseContext { get; set; }

    public Dictionary<string,List<string>> OutputTemplates { get; set; }

    public ProcessorOutput LastValue { get; set; }

    // A place for statements to write output (output template handling could use this later)
    public List<string> AdditionalOutput { get; set; }


    public StatementExecutionContect(ResponseGenerationContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      ResponseContext = context;
      OutputTemplates = new Dictionary<string, List<string>>();
      LastValue = null;
      AdditionalOutput = new List<string>();
    }


    public void AddOutputTemplate(KeyValuePair<string, string> template)
    {
      if (!OutputTemplates.ContainsKey(template.Key))
        OutputTemplates[template.Key] = new List<string>();
      OutputTemplates[template.Key].Add(template.Value);
    }


    public void Continue(Continuation c)
    {
      ResponseContext.Continue(c);
    }
  }
}
