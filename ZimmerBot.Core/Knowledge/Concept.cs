using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class Concept
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public string Name { get; protected set; }

    public ChoiceWRegex Choices { get; protected set; }



    public Concept(KnowledgeBase kb, string name, List<List<string>> patterns)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      KnowledgeBase = kb;
      Name = name;

      ConvertToWRegex(patterns);
    }


    protected void ConvertToWRegex(List<List<string>> patterns)
    {
      Choices = new ChoiceWRegex();

      foreach (List<string> pattern in patterns)
      {
        if (pattern.Count == 1)
        {
          WRegex wordRegex = ConvertWordToWRegex(pattern[0]);
          Choices.Add(wordRegex);
        }
        else
        {
          SequenceWRegex seqRegex = new SequenceWRegex();
          foreach (string word in pattern)
          {
            WRegex wordRegex = ConvertWordToWRegex(pattern[0]);
            seqRegex.Add(wordRegex);
          }
          Choices.Add(seqRegex);
        }
      }
    }


    protected WRegex ConvertWordToWRegex(string word)
    {
      if (word.StartsWith("%"))
      {
        string key = word.Substring(1);
        if (!KnowledgeBase.Concepts.ContainsKey(key))
          throw new InvalidOperationException($"The concept reference '{key}' in concept definition '{Name}' could not be found.");

        return KnowledgeBase.Concepts[key].Choices;
      }
      else
      {
        return new WordWRegex(word);
      }
    }
  }
}
