using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class Concept
  {
    public string Name { get; protected set; }

    public List<string> Words { get; protected set; }



    internal Concept(string name, IEnumerable<string> words)
    {
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(words, nameof(words)).IsNotNull();

      Name = name;
      Words = new List<string>(words);
    }


    public void ExpandToken(ZToken t)
    {
      foreach (string word in Words)
        if (t.OriginalText.Equals(word, StringComparison.InvariantCultureIgnoreCase))
          t.AddMatchingConcept(Name);
    }
  }
}
