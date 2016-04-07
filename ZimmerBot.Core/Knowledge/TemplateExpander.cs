using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class TemplateExpander : ITemplateExpander
  {
    KnowledgeBase KnowledgeBase { get; set; }

    Request OriginalRequest { get; set; }

    IDictionary<string, object> Variables { get; set; }


    public TemplateExpander(KnowledgeBase kb, Request originalRequest, IDictionary<string, object> variables)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(originalRequest, nameof(originalRequest)).IsNotNull();
      Condition.Requires(variables, nameof(variables)).IsNotNull();
      KnowledgeBase = kb;
      OriginalRequest = originalRequest;
      Variables = variables;
    }


    public string ExpandPlaceholdes(string s)
    {
      string output = TextMerge.MergeTemplate(s, Variables);
      return output;
    }


    public string Invoke(string s)
    {
      Request request = new Request(OriginalRequest, s);
      Response response = BotUtility.InvokeInternal(KnowledgeBase, request, false, true);
      return response.Output.Aggregate((a, b) => a + "\n" + b);
    }
  }
}
