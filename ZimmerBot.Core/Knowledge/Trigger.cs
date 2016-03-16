using System;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Trigger
  {
    protected WRegex Regex { get; set; }

    protected double RegexSize { get; set; }

    protected Expression Condition { get; set; }

    protected TimeSpan? Schedule { get; set; }


    public Trigger(params object[] topics)
    {
      SequenceWRegex p = new SequenceWRegex();

      foreach (object t in topics)
      {
        if (t is string)
          p.Add(new WordWRegex((string)t));
        else if (t is WRegex)
          p.Add((WRegex)t);
        else if (t == null)
          throw new ArgumentNullException("t", "Null item in topics");
        else
          throw new InvalidOperationException(string.Format("Cannot add {0} ({1} as trigger predicate.", t, t.GetType()));
      }

      Regex = p;
      RegexSize = p.CalculateSize();
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


    public void RegisterScheduledJobs(IScheduler scheduler, string ruleId)
    {
      if (Schedule != null)
      {
        IJobDetail job = JobBuilder.Create<ScheduledTriggerJob>()
          .UsingJobData("ruleId", ruleId)
          .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(Schedule.Value)
                .RepeatForever())
            .Build();

        scheduler.ScheduleJob(job, trigger);
      }
    }


    public WRegex.MatchResult CalculateTriggerScore(EvaluationContext context)
    {
      // FIXME: some mixing of concerns here - should be wrapped differently
      context.CurrentTokenIndex = 0;

      double conditionModifier = 1;

      if (Condition != null)
      {
        object value = Condition.Evaluate(context);
        if (value is bool)
          conditionModifier = ((bool)value) ? 1 : 0;
      }

      WRegex.MatchResult result = Regex.CalculateMatchResult(context, new EndOfSequenceWRegex());

      double totalScore = conditionModifier * result.Score * Math.Max(RegexSize,1);

      return new WRegex.MatchResult(result, totalScore, result.MatchedText);
    }


    private class ScheduledTriggerJob : IJob
    {
      public void Execute(IJobExecutionContext context)
      {
        string ruleId = context.JobDetail.JobDataMap.GetString("ruleId");
        Console.WriteLine("RD: " + ruleId);
        Rule r = RuleRepository.Get(ruleId);

        // FIXME: how can this be shared between Bot and Trigger ... what is missing?
        // - Swap it: save BotID and RuleID then lookup BOT and ask to run RuleID

        //EvaluationContext context = new EvaluationContext(State, input);
        //ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

        //if (reactions.Count > 0)
        //  foreach (Reaction r in reactions)
        //    output.Add(r.GenerateResponse(input));
        //else
        //  output.Add("???");

        //State["state.conversation.entries.Count"] = (double)State["state.conversation.entries.Count"] + 1;

      }
    }
  }
}
