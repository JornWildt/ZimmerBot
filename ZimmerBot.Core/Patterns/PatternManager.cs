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

    protected double WorstCaseUnknownWordProb { get; set; }

    protected double BestCaseUnknownWordProb { get; set; }

    protected double MediumUnknownWordProb { get; set; }


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
        entry.SetupComplete(KnowledgeBase);

      TotalNumberOfPatterns = PatternSets.Sum(p => p.Patterns.Count); ;
      TotalNumberOfWords = PatternSets.Sum(p => p.NumberOfWordsInPatternSet);

      foreach (var entry in PatternSets)
      {
        entry.UpdateStatistics(TotalNumberOfPatterns, TotalNumberOfWords);

        if (SpellChecker.IsInitialized)
          entry.ExtractWordsForSpellChecker();
      }

      if (PatternSets.Any())
      {
        WorstCaseUnknownWordProb = PatternSets.Min(ps => ps.UnknownWordProbability);
        BestCaseUnknownWordProb = PatternSets.Max(ps => ps.WordInPatternSetProbability.Max(p => p.Value));
        MediumUnknownWordProb = (WorstCaseUnknownWordProb + BestCaseUnknownWordProb) / 2.0;
      }
    }


    public PatternMatchResultList CalculateMostLikelyPattern(ZTokenSequenceList inputs)
    {
      if (PatternSets.Count == 0)
        return null;

      PatternMatchResultList result = new PatternMatchResultList();

      int inputTokenCount = inputs.Min(inp => inp.Count);

      // This value represents the best score found so far.
      // A perfect match is number-of-input-tokens * 1.0
      // - Start half way
      double maxScore = (double)inputTokenCount / (double)2;

      // Try all possible variations/reductions of the input
      foreach (ZTokenSequence input in inputs)
      {
        BotUtility.EvaluationLogger.Debug($"Trying to match input: {input}.");

        foreach (Pattern pt in PatternSets.SelectMany(ps => ps.Patterns))
        {
          double score = pt.CalculateScore(input, out List<string> reason);

          if (score > maxScore)
          {
            maxScore = score;
            result.Clear();
            // Add possible combinations of pattern identifiers as result
            foreach (var identifiers in pt.ParentPatternSet.UnfoldedIdentifiers)
            {
              PatternMatchResult r = new PatternMatchResult(pt, input, identifiers);
              result.Add(r);
              BotUtility.EvaluationLogger.Debug($"Better match: {r.ToString()} ({score})");
            }

            BotUtility.EvaluationLogger.Debug("  " + reason.Aggregate((a, b) => a + " | " + b));
          }
          else
          {
            double difference = Math.Abs(score / 1000000);
            // Are the values equal with respect to a small margin?
            if (Math.Abs(score - maxScore) <= difference)
            {
              // Add possible combinations of pattern identifiers as result
              foreach (var identifiers in pt.ParentPatternSet.UnfoldedIdentifiers)
              {
                PatternMatchResult r = new PatternMatchResult(pt, input, identifiers);
                result.Add(r);
                BotUtility.EvaluationLogger.Debug($"Similar match: {r.ToString()} ({score})");
              }
              BotUtility.EvaluationLogger.Debug("  " + reason.Aggregate((a, b) => a + " | " + b));
            }
          }
        }
      }

      return result;
    }


    public PatternMatchResultList CalculateMostLikelyPattern_V1(ZTokenSequenceList inputs)
    {
      if (PatternSets.Count == 0)
        return null;

      PatternMatchResultList result = new PatternMatchResultList();

      int inputTokenCount = inputs.Min(inp => inp.Count);

      double minimalAllowedProb = inputTokenCount * MediumUnknownWordProb * 2;

      // This value represents the best probability found so far - might as well start with the minimal allowed probability
      double maxProb = minimalAllowedProb;
      double nearestProb = maxProb;

      // Try all possible variations/reductions of the input
      foreach (ZTokenSequence input in inputs)
      {
        BotUtility.EvaluationLogger.Debug($"Trying to match input: {input}. MinP = {minimalAllowedProb}.");

        foreach (Pattern pt in PatternSets.SelectMany(ps => ps.RelevantPatternsForMatching))
        {
          double pb = pt.CalculateProbability(input, out List<string> reason);

          if (pb > maxProb)
          {
            nearestProb = maxProb;
            maxProb = pb;
            result.Clear();
            // Add possible combinations of pattern identifiers as result
            foreach (var identifiers in pt.ParentPatternSet.UnfoldedIdentifiers)
            {
              PatternMatchResult r = new PatternMatchResult(pt, input, identifiers);
              result.Add(r);
              BotUtility.EvaluationLogger.Debug($"Better match: {r.ToString()} ({pb})");
            }

            BotUtility.EvaluationLogger.Debug("  " + reason.Aggregate((a, b) => a + " | " + b));
          }
          else
          {
            double difference = Math.Abs(pb / 1000000);
            // Are the values equal with respect to a small margin?
            if (Math.Abs(pb - maxProb) <= difference)
            {
              // Add possible combinations of pattern identifiers as result
              foreach (var identifiers in pt.ParentPatternSet.UnfoldedIdentifiers)
              {
                PatternMatchResult r = new PatternMatchResult(pt, input, identifiers);
                result.Add(r);
                BotUtility.EvaluationLogger.Debug($"Similar match: {r.ToString()} ({pb})");
              }
              BotUtility.EvaluationLogger.Debug("  " + reason.Aggregate((a, b) => a + " | " + b));
            }
          }
        }
      }

      return result;
    }
  }
}
