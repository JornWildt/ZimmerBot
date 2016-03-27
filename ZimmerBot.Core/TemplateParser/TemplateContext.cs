using CuttingEdge.Conditions;

namespace ZimmerBot.Core.TemplateParser
{
  public interface ITemplateExpander
  {
    string ExpandPlaceholdes(string s);
    string Invoke(string s);
  }


  public class TemplateContext
  {
    public ITemplateExpander Expander { get; set; }

    public TemplateContext(ITemplateExpander expander)
    {
      Condition.Requires(expander, nameof(expander)).IsNotNull();
      Expander = expander;
    }
  }
}
