using System;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinition
  {
    public List<string> Words { get; protected set; }

    public List<string> Equivalents { get; protected set; }



    internal WordDefinition(IEnumerable<string> words)
    {
      Words = new List<string>(words);
      Equivalents = new List<string>();
    }


    internal WordDefinition(string word)
      : this(new string[] { word })
    {
    }


    public WordDefinition And(string word)
    {
      Words.Add(word);
      return this;
    }


    public WordDefinition Is(string key)
    {
      Equivalents.Add(key);
      return this;
    }


    public void ExpandToken(ZToken t)
    {
      foreach (string word in Words)
        if (t.OriginalText.Equals(word, StringComparison.InvariantCultureIgnoreCase))
          t.AddEquivalent(Equivalents);
    }
  }
}
