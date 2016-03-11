using System;
using System.Collections.Generic;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinition
  {
    public List<string> Words { get; protected set; }

    protected List<string> Equivalents { get; set; }



    internal WordDefinition(string word)
    {
      Words = new List<string>();
      Equivalents = new List<string>();
      Words.Add(word);
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


    public void ExpandToken(Token t)
    {
      foreach (string word in Words)
        if (t.OriginalText.Equals(word, StringComparison.InvariantCultureIgnoreCase))
          t.AddEquivalent(Equivalents);
    }
  }
}
