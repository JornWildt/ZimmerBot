using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class PatternRule : Rule
  {
    public StringPairList KeyValuePattern { get; protected set; }

    public string RequiredPriorRuleId { get; protected set; }


    public PatternRule(KnowledgeBase kb, string label, Topic topic, StringPairList pattern, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
      Condition.Requires(pattern, nameof(pattern)).IsNotNull();

      KeyValuePattern = pattern;
    }


    public override void RegisterParentRule(Rule parentRule)
    {
      RequiredPriorRuleId = parentRule.Id;
    }


    public override void SetupComplete()
    {
      // Do nothing
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight)
    {
      if (context.MatchedPattern == null)
        return new List<Reaction>();

      double conditionModifier = 1;

      if (RequiredPriorRuleId != null)
      {
        string lastRuleId = context.InputContext.State[StateKeys.SessionStore][StateKeys.LastRuleId] as string;
        if (lastRuleId is string)
        {
          if (RequiredPriorRuleId == lastRuleId)
            conditionModifier *= 4;
          else
            conditionModifier = 0;
        }
        else
          conditionModifier = 0;
      }

      if (conditionModifier == 0)
        return new List<Reaction>();

      bool ok = true;
      Dictionary<string, string> matchValues = context.MatchedPattern.MatchValues;

      foreach (var pair in KeyValuePattern)
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

      if (ok)
      {
        // Larger pattern sets are more difficult to match and thus prioritized higher
        // - but wildcard matches weight less
        double score = context.MatchedPattern.MatchPattern.Expressions.Sum(expr => expr.Weight);

        MatchResult result = new MatchResult(score * weight * conditionModifier);
        foreach (var item in matchValues)
          result.Matches[item.Key] = item.Value;

        //if (Weight != null)
        //result.Score = result.Score * Weight.Value;

        List<Reaction> reactions = SelectReactions(context, result);
        return reactions;
      }
      else
      {
        return new List<Reaction>();
      }
    }


    public override string ToString()
    {
      return KeyValuePattern.ToString();
    }
  }
}
