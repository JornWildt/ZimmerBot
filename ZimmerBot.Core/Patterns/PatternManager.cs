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
      double totalNumberOfPatterns = PatternSets.Sum(p => p.Patterns.Count); ;
      double totalNumberOfWords = PatternSets.Sum(p => p.NumberOfWords);

      foreach (var entry in PatternSets)
        entry.UpdateStatistics(totalNumberOfPatterns, totalNumberOfWords);
    }


    public PatternMatchResult CalculateMostLikelyPattern(ZTokenSequence input)
    {
      double minProb = double.MaxValue;
      double maxProb = 0.0;
      Pattern result = null;

      foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
      {
        double pb = pt.CalculateProbability(input);

        if (pb > maxProb)
        {
          maxProb = pb;
          result = pt;
        }

        if (pb < minProb)
          minProb = pb;
      }

      var rel = input.Count * maxProb / minProb;

      if (rel > 3.0) // FIXME: why this number? Make configurable
        return new PatternMatchResult(result, input);
      else
        return null;
    }
  }
}
