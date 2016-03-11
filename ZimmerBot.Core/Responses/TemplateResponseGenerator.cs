using System;
using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Responses
{
  public class TemplateResponseGenerator : ResponseGenerator
  {
    protected string Template { get; set; }


    public TemplateResponseGenerator(string template)
    {
      Template = template;
    }


    public override Func<string> Bind(Dictionary<string, string> input)
    {
      if (input.Count == 0)
        return () => Template;

      return () => Template + " (" + input.Select(item => "(" + item.Key + "|" + item.Value + ")").Aggregate((a, b) => a + "/" + b);
    }
  }
}
