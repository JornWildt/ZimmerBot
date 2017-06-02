using System;
using System.Collections.Generic;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public abstract class Trigger
  {
    public Expression Condition { get; protected set; }

    public string RequiredPriorRuleId { get; protected set; }


    public Trigger WithCondition(Expression c)
    {
      Condition = c;
      return this;
    }


    public void RegisterParentRule(Rule parentRule)
    {
      RequiredPriorRuleId = parentRule.Id;
    }


    public abstract void ExtractWordsForSpellChecker();

    public abstract void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId);

    public abstract MatchResult CalculateTriggerScore(TriggerEvaluationContext context);


    protected double CalculateConditionModifier(TriggerEvaluationContext context)
    {
      double conditionModifier = 1;

      if (Condition != null)
      {
        ExpressionEvaluationContext eec = new ExpressionEvaluationContext(context.InputContext.State.State);
        object value = Condition.Evaluate(eec);
        bool b;
        if (Expression.TryConvertToBool(value, out b))
          conditionModifier = (b ? 1 : 0);
        else
          throw new InvalidCastException($"Could not convert value '{value}' to bool in condition.");
      }

      //bool isAnswer = false;
      if (RequiredPriorRuleId != null)
      {
        string lastRuleId = context.InputContext.State[StateKeys.SessionStore][StateKeys.LastRuleId] as string;
        if (lastRuleId is string)
        {
          if (RequiredPriorRuleId == lastRuleId)
          {
            conditionModifier *= 4;
            //isAnswer = true;
          }
          else
            conditionModifier = 0;
        }
        else
          conditionModifier = 0;
      }

#if false
      // Do not trigger topic starters when already in topic
      // - unless input is an answer to a question in the topic starter
      if (!isAnswer)
      {
        if (context.StartingTopic != null)
        {
          if (context.InputContext.Session.CurrentTopic() == context.StartingTopic)
          {
            conditionModifier = 0;
          }
        }
      }
#endif

      return conditionModifier;
    }
  }
}
