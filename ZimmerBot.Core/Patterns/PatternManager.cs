using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

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
      {
        entry.UpdateStatistics(TotalNumberOfPatterns, TotalNumberOfWords);

        if (SpellChecker.IsInitialized)
          entry.ExtractWordsForSpellChecker();
      }
    }


    public PatternMatchResult CalculateMostLikelyPattern(ZTokenSequenceList inputs)
    {
      if (PatternSets.Count == 0)
        return null;

      BotUtility.EvaluationLogger.Debug($"Trying to match input: {inputs.ToString()}");

      Pattern result = null;
      ZTokenSequence resultInput = null;

      // Try all possible variations/reductions of the input
      foreach (ZTokenSequence input in inputs)
      {
        // This value approximates the smallest probability for a match 
        // (namely P(u)^N where N = number of un-matched words "u")
        double minimalAllowedProb = input.Count *
          Math.Log((1 / TotalNumberOfPatterns) * 1 / (TotalNumberOfWords + input.Count));

        minimalAllowedProb += Math.Log(input.Count);

        // This value represents the best probability found so far - might as well start with the minimal allowed probability
        double maxProb = minimalAllowedProb;

        foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
        {
          double pb = pt.CalculateProbability(input);

          if (pb > maxProb)
          {
            maxProb = pb;
            result = pt;
            resultInput = input;

            BotUtility.EvaluationLogger.Debug($"Probable match: {result.ToString()}");
          }
        }
      }

      if (result != null)
        return new PatternMatchResult(result, resultInput);
      else
        return null;
    }
  }
}
