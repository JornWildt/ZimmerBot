using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Statements
{
  public class OutputTemplateStatement : Statement
  {
    public KeyValuePair<string, string> Template { get; set; }

    public OutputTemplateStatement(KeyValuePair<string, string> template)
    {
      Condition.Requires(template.Key, nameof(template.Key)).IsNotNull();
      Condition.Requires(template.Value, nameof(template.Value)).IsNotNull();
      Template = template;
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      context.AddOutputTemplate(Template);
    }
  }
}
