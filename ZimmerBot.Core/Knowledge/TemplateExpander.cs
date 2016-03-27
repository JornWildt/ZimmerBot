using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class TemplateExpander : ITemplateExpander
  {
    KnowledgeBase KnowledgeBase { get; set; }

    BotState State { get; set; }

    IDictionary<string, object> Variables { get; set; }


    public TemplateExpander(KnowledgeBase kb, BotState state, IDictionary<string, object> variables)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(variables, nameof(variables)).IsNotNull();
      KnowledgeBase = kb;
      State = state;
      Variables = variables;
    }


    public string ExpandPlaceholdes(string s)
    {
      string output = TextMerge.MergeTemplate(s, Variables);
      return output;
    }


    public string Invoke(string s)
    {
      Request request = new Request { Input = s };
      Response response = BotUtility.Invoke(KnowledgeBase, State, request);
      return response.Output.Aggregate((a, b) => a + " " + b);
    }
  }
}
