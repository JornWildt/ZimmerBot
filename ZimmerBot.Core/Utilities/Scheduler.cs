﻿using System;
using Quartz;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public static class Scheduler
  {
    public static void AddDelayedMessage(IScheduler scheduler, DateTime at, string message, ResponseGenerationContext context)
    {
      IJobDetail job = JobBuilder.Create<ScheduledBotCallback>()
        .UsingJobData("Message", message)
        .UsingJobData("SessionId", context.Request.SessionId)
        .UsingJobData("BotId", context.Request.BotId)
        .Build();

      ITrigger trigger = TriggerBuilder.Create()
        .StartAt(at)
        .Build();

      scheduler.ScheduleJob(job, trigger);
    }
  }
}
