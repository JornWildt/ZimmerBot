using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl.Matchers;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Scheduler
{
  public class ScheduledBotCallback : IJob
  {
    static ILog Logger = LogManager.GetLogger(typeof(ScheduledBotCallback));


    public void Execute(IJobExecutionContext context)
    {
      Bot bot = null;
      string botId = null;

      try
      {
        // Extract what ever state was available when the callback was scheduled
        string message = context.JobDetail.JobDataMap.GetString("Message");
        string stateJson = context.JobDetail.JobDataMap.GetString("State");
        botId = context.JobDetail.JobDataMap.GetString("BotId");

        Dictionary<string, string> state = (stateJson != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(stateJson) : null);

        // Take the scheduled response and emit to the user
        Response response = new Response(new string[] { message }, state);
        bot = BotRepository.Get(botId);
        bot.SendResponse(response);
      }
      catch (Exception ex)
      {
        Logger.Error(ex);
      }

      try
      {
        // Try to mark as as ready even if errors occured in other part of code!
        string sessionId = context.JobDetail.JobDataMap.GetString("SessionId"); ;
        var groupMatcher = GroupMatcher<JobKey>.GroupContains(sessionId);
        var jobKeys = ScheduleHelper.DefaultScheduler.GetJobKeys(groupMatcher);
        bool isLastMessage = (jobKeys.Count == 1);

        if (isLastMessage)
        {
          // When the last message is output we mark the dialog as ready again
          Session session = SessionManager.GetSession(sessionId);
          session.MarkAsReadyForInput();

          // If user entered something during the monolog then react to the last statement
          string latestInput = session.GetLatestInput();

          // Make sure we do not fail because of a prior failure in this code
          if (bot != null && latestInput != null && botId != null)
          {
            Request req = new Request { Input = latestInput, BotId = botId, SessionId = sessionId, UserId = null };
            bot.Invoke(req, callbackToEnvironment: true);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Error(ex);
      }
    }
  }
}
