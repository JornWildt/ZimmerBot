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
      string stateJson = context.JobDetail.JobDataMap.GetString("State");
      string botId = context.JobDetail.JobDataMap.GetString("BotId");

      object state = (stateJson != null ? JsonConvert.DeserializeObject(stateJson) : null);

      Response response = new Response(new string[] { message }, state);
      Bot bot = BotRepository.Get(botId);
      bot.SendResponse(response);
    }
  }
}
