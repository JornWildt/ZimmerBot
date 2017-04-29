using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class ScheduledTrigger : Trigger
  {
    public TimeSpan? Schedule { get; protected set; }


    public ScheduledTrigger(TimeSpan interval)
    {
      Schedule = interval;
    }


    public override void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId)
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


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      //if (!context.ExecuteScheduledRules && Schedule != null)
        return new MatchResult(0);

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
