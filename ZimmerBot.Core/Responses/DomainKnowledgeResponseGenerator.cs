using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Responses
{
  public class DomainKnowledgeResponseGenerator : ResponseGenerator
  {
    protected KnowledgeBase KnowledgeBase { get; set; }


    public DomainKnowledgeResponseGenerator(KnowledgeBase kb)
    {
      KnowledgeBase = kb;
    }


    public override Func<string> Bind(Dictionary<string, string> input)
    {
      return () =>
        "Jeg ved noget om "
        + KnowledgeBase.GetDomains().Select(d => d.Name).Aggregate((a, b) => a + ", " + b);
    }
  }
}
