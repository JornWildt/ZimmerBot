using CuttingEdge.Conditions;

namespace ZimmerBot.Core.TemplateParser
{
  public interface ITemplateExpander
  {
    /// <summary>
    /// Expand StringTemplate placeholdes using StringTemplate.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    string ExpandPlaceholdes(string s);

    /// <summary>
    /// Re-invoke bot on supplied string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
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
