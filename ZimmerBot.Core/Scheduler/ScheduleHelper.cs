using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Scheduler
{
  public static class ScheduleHelper
  {
    public static IScheduler DefaultScheduler
    {
      get
      {
        return StdSchedulerFactory.GetDefaultScheduler();
      }
    }


    public static void AddScheduledAction(ScheduledAction action, string botId)
    {
      var trigger = TriggerBuilder.Create()
        .WithIdentity(action.Id)
        .WithCronSchedule(action.CronExpr)
        .Build();

      IJobDetail job = JobBuilder.Create<ScheduledActionJob>()
        .UsingJobData("BotId", botId)
        .UsingJobData("ActionId", action.Id)
        .Build();

      DefaultScheduler.ScheduleJob(job, trigger);
    }


    public static void AddDelayedMessage(
      DateTime at,
      string message,
      ResponseGenerationContext context)
    {
      string stateJson = (context.Request.State != null ? JsonConvert.SerializeObject(context.Request.State) : null);

      IJobDetail job = JobBuilder.Create<ScheduledMessageJob>()
        .WithIdentity(Guid.NewGuid().ToString(), context.Request.SessionId)
        .UsingJobData("Message", message)
        .UsingJobData("State", stateJson)
        .UsingJobData("SessionId", context.Request.SessionId)
        .UsingJobData("BotId", context.Request.BotId)
        .Build();

      ITrigger trigger = TriggerBuilder.Create()
        .StartAt(at)
        .Build();

      DefaultScheduler.ScheduleJob(job, trigger);

      context.Session.MarkAsBusyWritingAndNotReadyForInput();
    }


    public static void StopDelayedMessages(StatementExecutionContect context)
    {
      // Delete all pending jobs for this session
      string sessionId = context.ResponseContext.Session.SessionId;
      var groupMatcher = GroupMatcher<JobKey>.GroupContains(sessionId);
      var jobKeys = ScheduleHelper.DefaultScheduler.GetJobKeys(groupMatcher);
      foreach (var jobKey in jobKeys)
      {
        ScheduleHelper.DefaultScheduler.DeleteJob(jobKey);
      }

      // Mark the session as ready again
      Session session = SessionManager.GetSession(sessionId);
      session.MarkAsReadyForInput();
    }
  }
}
