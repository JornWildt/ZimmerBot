using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class Concept
  {
    public string Name { get; protected set; }

    public List<string> OriginalWords { get; protected set; }

    public ChoiceWRegex Choices { get; protected set; }



    public Concept(string name, IEnumerable<string> words)
    {
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(words, nameof(words)).IsNotNull();

      Name = name;
      OriginalWords = new List<string>(words);
    }


    public void ConvertToWRegex(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      List<WRegex> choices = new List<WRegex>();

      foreach (string word in OriginalWords)
      {
        if (word.StartsWith("%"))
        {
          string key = word.Substring(1);
          if (!kb.Concepts.ContainsKey(key))
            throw new InvalidOperationException($"The concept reference '{key}' in concept definition '{Name}' could not be found.");

          foreach (WRegex w in kb.Concepts[key].Choices.Choices)
            choices.Add(w);
        }
        else
        {
          choices.Add(new WordWRegex(word));
        }
      }

      Choices = new ChoiceWRegex(choices);
    }


    public void ExpandToken(ZToken t)
    {
      foreach (string word in OriginalWords)
        if (t.OriginalText.Equals(word, StringComparison.InvariantCultureIgnoreCase))
          t.AddMatchingConcept(Name);
    }
  }
}
