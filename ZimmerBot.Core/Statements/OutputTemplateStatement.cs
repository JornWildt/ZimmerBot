using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Statements
{
  public class OutputTemplateStatement : Statement
  {
    public OutputTemplate Template { get; set; }

    public OutputTemplateStatement(OutputTemplate template)
    {
      Condition.Requires(template, nameof(template)).IsNotNull();
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
