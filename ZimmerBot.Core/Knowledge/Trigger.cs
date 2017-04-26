using System;
using System.Collections.Generic;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    public WRegex Regex { get; protected set; }

    public double RegexSize { get; protected set; }

    public Expression Condition { get; protected set; }

    public TimeSpan? Schedule { get; protected set; }

    public string RequiredPriorRuleId { get; protected set; }


    public Trigger(WRegexBase pattern)
    {
      if (pattern != null)
      {
        Regex = new WRegex(pattern);
        RegexSize = pattern.CalculateSize();
      }
    }


    public Trigger(List<WRegexBase> patterns)
    {
      if (patterns == null || patterns.Count == 0 || patterns[0] == null)
      {
        Regex = null;
        RegexSize = 0;
      }
      else if (patterns.Count == 1)
      {
        Regex = new WRegex(patterns[0]);
        RegexSize = Regex.CalculateSize();
      }
      else if (patterns.Count > 1)
      {
        Regex = new WRegex(new ChoiceWRegex(patterns));
        RegexSize = Regex.CalculateSize();
      }
    }


    public Trigger(params object[] pattern)
    {
      if (pattern == null || pattern.Length == 0 || pattern[0] == null)
      {
        Regex = null;
        RegexSize = 0;
      }
      else if (pattern.Length == 1)
      {
        Regex = new WRegex(GetRegex(pattern[0]));
        RegexSize = Regex.CalculateSize();
      }
      else if (pattern.Length > 1)
      {
        SequenceWRegex p = new SequenceWRegex();

        foreach (object t in pattern)
        {
          WRegexBase r = GetRegex(t);
          p.Add(r);
        }

        Regex = new WRegex(p);
        RegexSize = p.CalculateSize();
      }
    }


    private WRegexBase GetRegex(object t)
    {
      if (t is string)
        return new LiteralWRegex((string)t);
      else if (t is WRegexBase)
        return (WRegexBase)t;
      else if (t == null)
        throw new ArgumentNullException("t", "Null item in topics");
      else
        throw new InvalidOperationException(string.Format("Cannot add {0} ({1} as trigger predicate.", t, t.GetType()));
    }


    public Trigger WithCondition(Expression c)
    {
      Condition = c;
      return this;
    }


    public Trigger WithSchedule(TimeSpan interval)
    {
      Schedule = interval;
      return this;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId)
    {
      if (Schedule != null)
      {
        IJobDetail job = JobBuilder.Create<ScheduledTriggerJob>()
          .UsingJobData("ruleId", ruleId)
          .UsingJobData("botId", botId)
          .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .StartAt(DateTime.Now.Add(Schedule.Value))
            .WithSimpleSchedule(x => x
                .WithInterval(Schedule.Value)
                .RepeatForever())
            .Build();

        scheduler.ScheduleJob(job, trigger);
      }
    }


    public void RegisterParentRule(Rule parentRule)
    {
      RequiredPriorRuleId = parentRule.Id;
    }


    public override string ToString()
    {
      return Regex != null ? Regex.ToString() + $" [Size: {RegexSize}]" : "<empty>";
    }


    public void ExtractWordsForSpellChecker()
    {
      Regex.ExtractWordsForSpellChecker();
    }


    public MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (!context.ExecuteScheduledRules && Schedule != null)
        return new MatchResult(0);

      context.CurrentRepetitionIndex = 1;

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

      MatchResult result;

      if (Regex != null)
      {
        if (context.InputContext.Input != null)
          result = Regex.CalculateMatch(context);
        else
          result = new MatchResult(0);
      }
      else
      {
        if (context.InputContext.Input != null)
          result = new MatchResult(0);
        else
          result = new MatchResult(1);
      }

      double totalScore = conditionModifier * result.Score * Math.Max(RegexSize,1);

      return new MatchResult(result, totalScore);
    }


    private class ScheduledTriggerJob : IJob
    {
      public void Execute(IJobExecutionContext context)
      {
        string ruleId = context.JobDetail.JobDataMap.GetString("ruleId");
        string botId = context.JobDetail.JobDataMap.GetString("botId");

        Bot b = BotRepository.Get(botId);
        b.Invoke(new Request { Input = null, RuleId = ruleId }, executeScheduledRules: true, callbackToEnvironment: true);
      }
    }
  }
}
