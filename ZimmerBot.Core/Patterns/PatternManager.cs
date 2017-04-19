using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class PatternManager
  {
    public List<PatternSet> PatternSets { get; protected set; }


    public PatternManager()
    {
      PatternSets = new List<PatternSet>();
    }


    public void AddPatternSet(PatternSet set)
    {
      Condition.Requires(set, nameof(set)).IsNotNull();

      PatternSets.Add(set);
    }


    public void UpdateStatistics()
    {
      double totalNumberOfWords = PatternSets.Sum(c => c.NumberOfWords);
      foreach (var entry in PatternSets)
        entry.UpdateStatistics(totalNumberOfWords);
    }


    public Pattern CalculateMostLikelyPattern(ZTokenSequence input)
    {
      double prob = 0.0;
      Pattern result = null;

      foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
      {
        double pb = pt.CalculateProbability(input);
        if (pb > prob)
        {
          prob = pb;
          result = pt;
        }
      }

      return result;
    }
  }
}
