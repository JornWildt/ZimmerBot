using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Patterns
{
  public class PatternSet
  {
    public List<KeyValuePair<string,List<string>>> Identifiers { get; protected set; }

    public List<List<KeyValuePair<string, string>>> UnfoldedIdentifiers { get; protected set; }

    //public HashSet<String> ReferencedIdentifiers { get; protected set; }

    public List<Pattern> Patterns { get; protected set; }

    public Dictionary<string, double> WordInPatternSetProbability { get; set; }

    public double UnknownWordProbability { get; set; }


    private double? _numberOfWords;
    public double NumberOfWordsInPatternSet
    {
      get
      {
        if (_numberOfWords == null)
        {
          _numberOfWords = Patterns.Sum(p => p.NumberOfWords);
        }
        return _numberOfWords.Value;
      }
    }


    public PatternSet(List<KeyValuePair<string, List<string>>> identifiers, List<Pattern> patterns)
    {
      Condition.Requires(identifiers, nameof(identifiers)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      Identifiers = identifiers;
      Patterns = patterns;

      foreach (Pattern p in Patterns)
        p.RegisterParent(this);
    }


    public void SetupComplete(KnowledgeBase kb)
    {
      ExpandPatterns(kb);
      UnfoldIdentifiers();
    }


    public void ExpandPatterns(KnowledgeBase kb)
    {
      Pattern[] existingPatterns = Patterns.ToArray();
      Patterns.Clear();

      foreach (Pattern p in existingPatterns)
      {
        p.ExpandExpressions(kb, Patterns);
      }

      //ReferencedIdentifiers = new HashSet<string>();
      //foreach (Pattern p in Patterns)
      //{
      //  foreach (string id in p.ReferencedParameters)
      //    ReferencedIdentifiers.Add(id);
      //}
    }


    public void UnfoldIdentifiers()
    {
      UnfoldedIdentifiers = new List<List<KeyValuePair<string, string>>>();

      UnfoldIdentifiers(0, new Stack<KeyValuePair<string,string>>(), UnfoldedIdentifiers);
    }


    private void UnfoldIdentifiers(
      int i,
      Stack<KeyValuePair<string, string>> set,
      List<List<KeyValuePair<string, string>>> result)
    {
      if (i < Identifiers.Count)
      {
        if (Identifiers[i].Value != null)
        {
          foreach (string id in Identifiers[i].Value)
          {
            set.Push(new KeyValuePair<string, string>(Identifiers[i].Key, id));
            UnfoldIdentifiers(i + 1, set, result);
            set.Pop();
          }
        }
        else
        {
          set.Push(new KeyValuePair<string, string>(Identifiers[i].Key, null));
          UnfoldIdentifiers(i + 1, set, result);
          set.Pop();
        }
      }
      else
      {
        result.Add(new List<KeyValuePair<string, string>>(set));
      }
    }


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      WordInPatternSetProbability = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

      foreach (var pattern in Patterns)
        pattern.InitializeStatistics(totalNumberOfPatterns, totalNumberOfWords);

      // All patterns have equal probability since this system have no apriori knowledge of actual usage patterns
      double patternProbability = 1.0 / totalNumberOfPatterns;

      // Normalize (at this point WordInPatternSetProbability[key] holds the count of "key").
      foreach (string key in WordInPatternSetProbability.Keys.ToArray())
      {
        WordInPatternSetProbability[key]
          = Math.Log(patternProbability * (WordInPatternSetProbability[key] + 1) / (NumberOfWordsInPatternSet + totalNumberOfWords));
      }

      // Probability for completely unknown words
      UnknownWordProbability = Math.Log(patternProbability * 1 / (NumberOfWordsInPatternSet + totalNumberOfWords));

      foreach (var pattern in Patterns)
        pattern.UpdateStatistics(totalNumberOfPatterns, totalNumberOfWords);
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (var pattern in Patterns)
        pattern.ExtractWordsForSpellChecker();
    }


    protected List<Pattern> _relevantPatternsForMatching;

    public IEnumerable<Pattern> RelevantPatternsForMatching
    {
      get
      {
        if (_relevantPatternsForMatching == null)
        {
          _relevantPatternsForMatching = Patterns.Where(p => p.HasWildcardExpression).ToList();
          if (_relevantPatternsForMatching.Count == 0)
            _relevantPatternsForMatching.Add(Patterns.First(p => !p.HasWildcardExpression));
        }
        return _relevantPatternsForMatching;
      }
    }
  }
}
