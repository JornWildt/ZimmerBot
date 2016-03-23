using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.ConfigParser
{
  public class TemplateOutputStatement : OutputStatement
  {
    public KeyValuePair<string, string> Template { get; set; }

    public TemplateOutputStatement(KeyValuePair<string, string> template)
    {
      Condition.Requires(template.Key, nameof(template.Key)).IsNotNull();
      Condition.Requires(template.Value, nameof(template.Value)).IsNotNull();
      Template = template;
    }

    public override void Execute(OutputExecutionContect context)
    {
      context.AddOutputTemplate(Template);
    }
  }
}
