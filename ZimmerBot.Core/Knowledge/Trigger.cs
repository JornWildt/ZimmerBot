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


    public Trigger(params object[] topics)
    {
      if (topics.Length == 1)
      {
        Regex = GetRegex(topics[0]);
        RegexSize = Regex.CalculateSize();
      }
      else if (topics.Length > 1)
      {
        SequenceWRegex p = new SequenceWRegex();

        foreach (object t in topics)
        {
          WRegex r = GetRegex(t);
          p.Add(r);
        }

        Regex = p;
        RegexSize = p.CalculateSize();
      }
      else
      {
        Regex = null;
        RegexSize = 0;
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
            //.StartNow()
            .StartAt(DateTime.Now.Add(Schedule.Value))
            .WithSimpleSchedule(x => x
                .WithInterval(Schedule.Value)
                .RepeatForever())
            .Build();

        scheduler.ScheduleJob(job, trigger);
      }
    }


    public WRegex.MatchResult CalculateTriggerScore(EvaluationContext context)
    {
      if (!context.ExecuteScheduledRules && Schedule != null)
        return new WRegex.MatchResult(0, "");

      // FIXME: some mixing of concerns here - should be wrapped differently
      context.CurrentTokenIndex = 0;
      context.CurrentRepetitionIndex = 1;


      double conditionModifier = 1;

      if (Condition != null)
      {
        // FIXME: make bot state available
        ExpressionEvaluationContext eec = new ExpressionEvaluationContext();
        object value = Condition.Evaluate(eec);
        if (value is bool)
          conditionModifier = ((bool)value) ? 1 : 0;
      }

      WRegex.MatchResult result;

      if (Regex != null)
      {
        if (context.Input != null)
          result = Regex.CalculateMatchResult(context, new EndOfSequenceWRegex());
        else
          result = new WRegex.MatchResult(0, "");
      }
      else
      {
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
