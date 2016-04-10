using System;
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


    public Trigger(params object[] pattern)
    {
      if (pattern == null || pattern.Length == 0 || pattern[0] == null)
      {
        Regex = null;
        RegexSize = 0;
      }
      else if (pattern.Length == 1)
      {
        Regex = GetRegex(pattern[0]);
        RegexSize = Regex.CalculateSize();
      }
      else if (pattern.Length > 1)
      {
        SequenceWRegex p = new SequenceWRegex();

        foreach (object t in pattern)
        {
          WRegex r = GetRegex(t);
          p.Add(r);
        }

        Regex = p;
        RegexSize = p.CalculateSize();
      }
    }


    private WRegex GetRegex(object t)
    {
      if (t is string)
        return new WordWRegex((string)t);
      else if (t is WRegex)
        return (WRegex)t;
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


    public WRegex.MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      if (!context.ExecuteScheduledRules && Schedule != null)
        return new WRegex.MatchResult(0, "");

      context.CurrentTokenIndex = 0;
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
            conditionModifier /= 4;
        }
        else
          conditionModifier /= 4;
      }

      WRegex.MatchResult result;

      if (Regex != null)
      {
        if (context.InputContext.Input != null)
          result = Regex.CalculateMatchResult(context, new EndOfSequenceWRegex());
        else
          result = new WRegex.MatchResult(0, "");
      }
      else
      {
        if (context.InputContext.Input != null)
          result = new WRegex.MatchResult(0.1, "");
        else
          result = new WRegex.MatchResult(1, "");
      }

      double totalScore = conditionModifier * result.Score * Math.Max(RegexSize,1);

      return new WRegex.MatchResult(result, totalScore, result.MatchedText);
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
