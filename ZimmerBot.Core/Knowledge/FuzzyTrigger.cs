﻿using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Quartz;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;
using Cond = CuttingEdge.Conditions.Condition;

namespace ZimmerBot.Core.Knowledge
{
  public class FuzzyTrigger : Trigger
  {
    public List<StringPairList> KeyValuePatterns { get; protected set; }


    public FuzzyTrigger(List<StringPairList> patterns)
    {
      Cond.Requires(patterns, nameof(patterns)).IsNotEmpty();
      KeyValuePatterns = patterns;
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing - handled by the pattern definitions
    }


    public override void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId)
    {
      // Do nothing
    }


    public override string ToString()
    {
      return KeyValuePatterns.Select(p => p.ToString()).Aggregate((a,b) => a + "|" + b);
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (context.MatchedPattern == null)
        return new MatchResult(0);

      MatchResult bestResult = null;

      foreach (StringPairList pattern in KeyValuePatterns)
      {
        MatchResult result = CalculateTriggerScore(context, pattern);
        if (bestResult == null || result.Score > bestResult.Score)
          bestResult = result;
      }

      double conditionModifier = CalculateConditionModifier(context);

      return new MatchResult(bestResult, bestResult.Score * conditionModifier);
    }


    protected MatchResult CalculateTriggerScore(TriggerEvaluationContext context, StringPairList pattern)
    {
      bool ok = true;
      Dictionary<string, string> matchValues = context.MatchedPattern.MatchValues;

      foreach (var pair in pattern)
      {
        bool pairOk = false;
        if (matchValues.ContainsKey(pair.Key))
        {
          string value = matchValues[pair.Key];
          if (pair.Value == Constants.StarValue || pair.Value == value)
            pairOk = true;
        }

        if (!pairOk)
          ok = false;
      }

      if (!ok)
        return new MatchResult(0);

      // Larger pattern sets are more difficult to match and thus prioritized higher
      // - but wildcard matches weight less
      double score = context.MatchedPattern.MatchPattern.Expressions.Sum(expr => expr.Weight) 
        + pattern.Sum(p => p.Value == Constants.StarValue ? 0.5 : 1.0);
      //double score = context.MatchedPattern.MatchPattern.Expressions.Sum(expr => expr.Weight);

      MatchResult result = new MatchResult(score);
      foreach (var item in matchValues)
        result.Matches[item.Key] = item.Value;

      return result;
    }
  }
}
