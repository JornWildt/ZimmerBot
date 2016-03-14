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

    protected List<string> Equivalents { get; set; }


    public ZToken(string t)
      : this(t, TokenType.Word)
    {
    }


    public ZToken(string t, TokenType type)
    {
      OriginalText = t;
      Type = type;
      Equivalents = new List<string>();
      Equivalents.Add(t);
    }


    public void AddEquivalents(string equivalent)
    {
      Equivalents.Add(equivalent);
    }

    public void AddEquivalent(IEnumerable<string> equivalents)
    {
      foreach (string m in equivalents)
        AddEquivalents(m);
    }


    public bool Matches(string v)
    {
      // No equivalents found => ignore
      if (Equivalents.Count == 0)
        return true;

      return Equivalents.Any(m => m == v);
    }


    public override string ToString()
    {
      return "(" + Equivalents.Aggregate((a,b) => a+"|"+b) +")";
    }


    public void ExtractParameter(HashSet<string> parameterMap, Dictionary<string, string> generatorParameters)
    {
      foreach (string s in Equivalents)
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
