using Quartz;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public class ScheduledBotCallback : IJob
  {
    public void Execute(IJobExecutionContext context)
    {
      string message = context.JobDetail.JobDataMap.GetString("Message");
      string botId = context.JobDetail.JobDataMap.GetString("BotId");

      Response response = new Response(new string[] { message }, null); // FIXME: state is needed!
      Bot bot = BotRepository.Get(botId);
      bot.SendResponse(response);
    }
  }
}
