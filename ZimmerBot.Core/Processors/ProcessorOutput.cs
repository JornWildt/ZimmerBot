using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Processors
{
  public class ProcessorOutput
  {
    public string TemplateName { get; protected set; }

    public object Value { get; protected set; }


    public ProcessorOutput(object value)
      : this("default", value)
    {
    }


    public ProcessorOutput(string templateName, object value)
    {
      Condition.Requires(templateName, nameof(templateName)).IsNotNullOrEmpty();
      TemplateName = templateName;
      Value = value;
    }
  }
}
