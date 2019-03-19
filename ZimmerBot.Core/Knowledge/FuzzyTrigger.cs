using CuttingEdge.Conditions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Patterns;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;
using Cond = CuttingEdge.Conditions.Condition;

namespace ZimmerBot.Core.Knowledge
{
  public class FuzzyTrigger : Trigger
  {
    private static ILog Logger = LogManager.GetLogger(typeof(FuzzyTrigger));

    public List<OperatorKeyValueList> KeyValuePatterns { get; protected set; }


    public FuzzyTrigger(List<OperatorKeyValueList> patterns)
    {
      Cond.Requires(patterns, nameof(patterns)).IsNotEmpty();
      KeyValuePatterns = patterns;
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing - handled by the pattern definitions
    }


    public override string ToString()
    {
      return KeyValuePatterns.Select(p => p.ToString()).Aggregate((a,b) => a + "|" + b);
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (context.MatchedPatterns == null)
        return new MatchResult(0);

      MatchResult bestResult = null;

      foreach (OperatorKeyValueList pattern in KeyValuePatterns)
      {
        MatchResult result = CalculateTriggerScore(context, pattern);
        if (bestResult == null || result.Score > bestResult.Score)
          bestResult = result;
      }

      double conditionModifier = CalculateConditionModifier(context);

      return new MatchResult(bestResult, bestResult.Score * conditionModifier);
    }


    protected MatchResult CalculateTriggerScore(TriggerEvaluationContext context, OperatorKeyValueList pattern)
    {
      MatchResult result = new MatchResult(0.0);

      foreach (PatternMatchResult matchedPattern in context.MatchedPatterns)
      {
        bool ok = true;
        Dictionary<string, ZToken> matchValues = matchedPattern.MatchValues;

        foreach (OperatorKeyValue pair in pattern)
        {
          bool pairOk = false;
          if (matchValues.ContainsKey(pair.Key))
          {
            ZToken value = matchValues[pair.Key];
            if (
              value == null
              || pair.Value == Constants.StarValue
              || (pair.Operator == "=" && value.OriginalText.Equals(pair.Value, StringComparison.CurrentCultureIgnoreCase))
              || (pair.Operator == ":" && value.Type == ZToken.TokenType.Entity && value.EntityClass.Equals(pair.Value, StringComparison.CurrentCultureIgnoreCase)))
              pairOk = true;
          }

          if (!pairOk)
            ok = false;
        }

        if (ok)
        {
          // Larger pattern sets are more difficult to match and thus prioritized higher
          double score = matchedPattern.MatchPattern.Expressions.Sum(expr => expr.Weight);

          // Larger pattern parameter sets are more difficult to match and thus prioritized higher
          if (pattern.Count > 0)
            score *= Math.Pow(1.1, pattern.Count-1);

          Logger.Debug($"Matched {pattern} with {matchedPattern}. Score = {score}");

          if (score > result.Score)
          {
            result = new MatchResult(score);
            foreach (var item in matchValues)
              result.Matches[item.Key] = item.Value;
          }
        }
      }

      return result;
    }
  }
}
