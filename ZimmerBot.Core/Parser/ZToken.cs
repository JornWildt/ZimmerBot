using System;
using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Parser
{
  public class ZToken
  {
    public enum TokenType { Word, Number, EMail }

    public string OriginalText { get; protected set; }

    public TokenType Type { get; protected set; }

    protected List<string> Concepts { get; set; }


    public ZToken(string t)
      : this(t, TokenType.Word)
    {
    }


    public ZToken(string t, TokenType type)
    {
      OriginalText = t;
      Type = type;
      Concepts = new List<string>();
      Concepts.Add(t);
    }


    public void AddMatchingConcept(string concept)
    {
      Concepts.Add(concept);
    }

    public void AddMatchingConcepts(IEnumerable<string> concepts)
    {
      foreach (string m in concepts)
        AddMatchingConcept(m);
    }


    public bool Matches(string v)
    {
      // No concepts found => ignore
      if (Concepts.Count == 0)
        return true;

      return Concepts.Any(m => m.Equals(v, StringComparison.CurrentCultureIgnoreCase));
    }


    public override string ToString()
    {
      return "(" + Concepts.Aggregate((a,b) => a+"|"+b) +")";
    }


    public void ExtractParameter(HashSet<string> parameterMap, Dictionary<string, string> generatorParameters)
    {
      foreach (string s in Concepts)
      {
        if (parameterMap.Contains(s))
        {
          generatorParameters.Add(s, OriginalText);
          break;
        }
      }
    }
  }
}
