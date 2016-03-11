using System;
using System.Collections.Generic;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinition
  {
    public string Word { get; protected set; }

    protected List<string> Equivalents { get; set; }



    internal WordDefinition(string word)
    {
      Word = word;
      Equivalents = new List<string>();
    }


    public WordDefinition And(string word)
    {
      //Equivalents.Add(key);
      return this;
    }


    public WordDefinition Is(string key)
    {
      Equivalents.Add(key);
      return this;
    }


    public void ExpandToken(Token t)
    {
      if (t.OriginalText.Equals(Word, StringComparison.InvariantCultureIgnoreCase))
        t.AddEquivalent(Equivalents);
    }
  }
}
