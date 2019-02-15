using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class TemplateExpander : ITemplateExpander
  {
    static Random Randomizer = new Random();

    protected ResponseGenerationContext ResponseContext { get; set; }


    public TemplateExpander(ResponseGenerationContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      ResponseContext = context;
    }


    public string ExpandPlaceholders(string s)
    {
      string output = TextMerge.MergeTemplate(s, ResponseContext.Variables);
      return output;
    }


    public string ExpandConcept(string conceptId)
    {
      if (ResponseContext.KnowledgeBase.Concepts.TryGetValue(conceptId, out Concept concept))
      {
        List<string> words = concept.ExpandPatterns().ToList();
        return words[Randomizer.Next(words.Count)];
      }
      else
        return "UNDEFINED CONCEPT: " + conceptId;
    }


    public string Invoke(string s)
    {
      Request request = new Request(ResponseContext.Request, s);
      List<string> output = new List<string>();

      BotUtility
        .InvokeStatements(ResponseContext.InputContext.RequestContext, request, fromTemplate: true, output: output);

      return output.Aggregate((a, b) => a + "\n" + b);
    }
  }
}
