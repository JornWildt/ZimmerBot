using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class Concept
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public string Name { get; protected set; }

    public List<List<string>> OriginalPatterns { get; protected set; }

    public ChoiceWRegex Choices { get; protected set; }



    public Concept(KnowledgeBase kb, string name, List<List<string>> patterns)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(name, nameof(name)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      KnowledgeBase = kb;
      Name = name;
      OriginalPatterns = patterns;

      ConvertToWRegex(patterns);
    }


    protected void ConvertToWRegex(List<List<string>> patterns)
    {
      Choices = new ChoiceWRegex();

      foreach (List<string> pattern in patterns)
      {
        if (pattern.Count == 1)
        {
          WRegexBase wordRegex = ConvertWordToWRegex(pattern[0]);
          Choices.Add(wordRegex);
        }
        else
        {
          SequenceWRegex seqRegex = new SequenceWRegex();
          foreach (string word in pattern)
          {
            WRegexBase wordRegex = ConvertWordToWRegex(word);
            seqRegex.Add(wordRegex);
          }
          Choices.Add(seqRegex);
        }
      }
    }


    protected WRegexBase ConvertWordToWRegex(string word)
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
        return new LiteralWRegex(word);
      }
    }


    public bool IsConceptMatch(ZTokenSequence input, int i, int j)
    {
      WRegexBase.EvaluationContext context = new WRegexBase.EvaluationContext(input, i, j);

      MatchResult result = Choices.CalculateNFAMatch(context);
      if (result.Score > 0)
        return true;

      return false;
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (string word in ExpandPatterns())
        SpellChecker.AddWord(word);
    }


    public IEnumerable<string> ExpandPatterns()
    {
      foreach (List<string> pattern in OriginalPatterns)
      {
        // FIXME: does not expand "%x %y", only "%x"
        string word = pattern.Aggregate((a, b) => a + " " + b);
        foreach (string w in ExpandWord(word))
          yield return w;
      }
    }


    public IEnumerable<string> ExpandWord(string word)
    {
      if (word.StartsWith("%"))
      {
        string key = word.Substring(1);
        if (!KnowledgeBase.Concepts.ContainsKey(key))
          throw new InvalidOperationException($"The concept reference '{key}' in concept definition '{Name}' could not be found.");

        foreach (string w in KnowledgeBase.Concepts[key].ExpandPatterns())
          yield return w;
      }
      else
        yield return word;
    }


    public override string ToString()
    {
      return Name;
    }
  }
}
