using System;
using Newtonsoft.Json;
using Quartz;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public static class Scheduler
  {
    public static void AddDelayedMessage(IScheduler scheduler, DateTime at, string message, ResponseGenerationContext context, bool nextWorking)
    {
      string stateJson = (context.Request.State != null ? JsonConvert.SerializeObject(context.Request.State) : null);

      IJobDetail job = JobBuilder.Create<ScheduledBotCallback>()
        .UsingJobData("Message", message)
        .UsingJobData("State", stateJson)
        .UsingJobData("SessionId", context.Request.SessionId)
        .UsingJobData("BotId", context.Request.BotId)
        .UsingJobData("NextWorking", nextWorking)
        .Build();

      ITrigger trigger = TriggerBuilder.Create()
        .StartAt(at)
        .Build();

      scheduler.ScheduleJob(job, trigger);
    }
  }
}
