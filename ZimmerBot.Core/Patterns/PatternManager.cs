using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class PatternManager
  {
    public List<PatternSet> PatternSets { get; protected set; }

    protected double TotalNumberOfPatterns { get; set; }

    protected double TotalNumberOfWords { get; set; }

    protected KnowledgeBase KnowledgeBase { get; set; }


    public PatternManager(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      PatternSets = new List<PatternSet>();
      KnowledgeBase = kb;
    }


    public void AddPatternSet(PatternSet set)
    {
      Condition.Requires(set, nameof(set)).IsNotNull();

      PatternSets.Add(set);
    }


    public void SetupComplete()
    {
      foreach (var entry in PatternSets)
        entry.ExpandPatterns(KnowledgeBase);

      TotalNumberOfPatterns = PatternSets.Sum(p => p.Patterns.Count); ;
      TotalNumberOfWords = PatternSets.Sum(p => p.NumberOfWords);

      foreach (var entry in PatternSets)
        entry.UpdateStatistics(TotalNumberOfPatterns, TotalNumberOfWords);
    }


    public PatternMatchResult CalculateMostLikelyPattern(ZTokenSequence input)
    {
      if (PatternSets.Count == 0)
        return null;

      double maxProb = -10000;
      Pattern result = null;

      foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
      {
        double pb = pt.CalculateProbability(input);

        if (pb > maxProb)
        {
          maxProb = pb;
          result = pt;
        }
      }

      // This value approximates the smallest probability for a match 
      // (namely P(u)^N where N = number of un-matched words "u")
      double minProb = input.Count * 
        Math.Log((1 / TotalNumberOfPatterns) * 1 / (TotalNumberOfWords + input.Count));

      if (maxProb > minProb + Math.Log(input.Count))
        return new PatternMatchResult(result, input);
      else
        return null;
    }
  }
}
