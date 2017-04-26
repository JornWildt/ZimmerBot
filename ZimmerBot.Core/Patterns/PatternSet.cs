using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Patterns
{
  public class PatternSet
  {
    public List<KeyValuePair<string,string>> Identifiers { get; protected set; }

    public List<Pattern> Patterns { get; protected set; }


    private double? _numberOfWords;
    public double NumberOfWords
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


    public PatternSet(List<KeyValuePair<string, string>> identifiers, List<Pattern> patterns)
    {
      Condition.Requires(identifiers, nameof(identifiers)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      Identifiers = identifiers;
      Patterns = patterns;

      foreach (Pattern p in Patterns)
        p.RegisterParent(this);
    }


    public void ExpandPatterns(KnowledgeBase kb)
    {
      Pattern[] existingPatterns = Patterns.ToArray();
      Patterns.Clear();

      foreach (Pattern p in existingPatterns)
      {
        p.ExpandExpressions(kb, Patterns);
      }
    }

    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      foreach (var pattern in Patterns)
        pattern.UpdateStatistics(totalNumberOfPatterns, totalNumberOfWords);
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (var pattern in Patterns)
        pattern.ExtractWordsForSpellChecker();
    }
  }
}
