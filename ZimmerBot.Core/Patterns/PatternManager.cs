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


    public PatternMatchResultList CalculateMostLikelyPattern(ZTokenSequenceList inputs)
    {
      if (PatternSets.Count == 0)
        return null;

      BotUtility.EvaluationLogger.Debug($"Trying to match input: {inputs.ToString()}");

      PatternMatchResultList result = new PatternMatchResultList();

      int tokenCount = inputs.Min(inp => inp.Count);

      // This value approximates the smallest probability for a match 
      // (namely P(u)^N where N = number of un-matched words "u")
      double minimalAllowedProb = tokenCount *
        Math.Log((1 / TotalNumberOfPatterns) * 1 / (TotalNumberOfWords + tokenCount));

      minimalAllowedProb += Math.Log(tokenCount + 1);

      // This value represents the best probability found so far - might as well start with the minimal allowed probability
      double maxProb = minimalAllowedProb;

      // Try all possible variations/reductions of the input
      foreach (ZTokenSequence input in inputs)
      {
        foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
        {
          double pb = pt.CalculateProbability(input);

          if (pb > maxProb)
          {
            maxProb = pb;
            PatternMatchResult r = new PatternMatchResult(pt, input);
            result.Clear();
            result.Add(r);

            BotUtility.EvaluationLogger.Debug($"Better match: {r.ToString()}");
          }
          else
          {
            double difference = Math.Abs(pb / 1000000);
            // Are the values equal with respect to a small margin?
            if (Math.Abs(pb - maxProb) <= difference)
            {
              PatternMatchResult r = new PatternMatchResult(pt, input);
              result.Add(r);
              BotUtility.EvaluationLogger.Debug($"Similar match: {r.ToString()}");
            }
          }
        }
      }

      if (result != null)
        return result;
      else
        return null;
    }
  }
}
