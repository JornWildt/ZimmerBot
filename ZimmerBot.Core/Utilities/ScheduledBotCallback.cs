using System.Collections.Generic;
using Newtonsoft.Json;
using Quartz;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public class ScheduledBotCallback : IJob
  {
    public void Execute(IJobExecutionContext context)
    {
      string message = context.JobDetail.JobDataMap.GetString("Message");
      string sessionId = context.JobDetail.JobDataMap.GetString("SessionId"); ;
      string stateJson = context.JobDetail.JobDataMap.GetString("State");
      string botId = context.JobDetail.JobDataMap.GetString("BotId");
      bool lastMessage = context.JobDetail.JobDataMap.GetBoolean("LastMessage");

      Dictionary<string, string> state = (stateJson != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(stateJson) : null);

      Response response = new Response(new string[] { message }, state);
      Bot bot = BotRepository.Get(botId);
      bot.SendResponse(response);

      Session session = SessionManager.GetSession(sessionId);
      if (lastMessage)
        BotUtility.MarkAsReady(session);
    }
  }
}
