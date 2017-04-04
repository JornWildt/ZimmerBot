using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.TemplateParser
{
  public static class TemplateUtility
  {
    public static SequenceTemplateToken Parse(string template)
    {
      TemplateParser parser = new TemplateParser();
      parser.Parse(template);
      return parser.Result;
    }


    public static string Merge(string template, ITemplateExpander expander)
    {
      Condition.Requires(expander, nameof(expander)).IsNotNull();

      SequenceTemplateToken tokens = Parse(template);
      TemplateContext context = new TemplateContext(expander);
      string output = tokens.Instantiate(context);

      return output;
    }
  }
}