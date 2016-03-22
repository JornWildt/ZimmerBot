using CuttingEdge.Conditions;


namespace ZimmerBot.Core.ConfigParser
{
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
}
