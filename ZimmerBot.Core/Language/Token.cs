using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Language
{
  public class Token
  {
    public string OriginalText { get; set; }

    protected List<string> Equivalents { get; set; }


    public Token(string t)
    {
      OriginalText = t;
      Equivalents = new List<string>();
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
  }
}
