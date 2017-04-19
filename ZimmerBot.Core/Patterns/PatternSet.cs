using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

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


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      foreach (var pattern in Patterns)
        pattern.UpdateStatistics(totalNumberOfPatterns, totalNumberOfWords);
    }
  }
}
