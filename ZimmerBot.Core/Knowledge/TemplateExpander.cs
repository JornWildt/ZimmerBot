using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class TemplateExpander : ITemplateExpander
  {
    protected ResponseContext ResponseContext { get; set; }


    public TemplateExpander(ResponseContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      ResponseContext = context;
    }


    public string ExpandPlaceholdes(string s)
    {
      string output = TextMerge.MergeTemplate(s, ResponseContext.Variables);
      return output;
    }


    public string Invoke(string s)
    {
      Request request = new Request(ResponseContext.OriginalRequest, s);
      List<string> output = new List<string>();

      BotUtility
        .InvokeStatements(
          request,
          ResponseContext.KnowledgeBase,
          ResponseContext.Session,
          ResponseContext.State, 
          fromTemplate: true, 
          output: output);

      return output.Aggregate((a, b) => a + "\n" + b);
    }
  }
}
