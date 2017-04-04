using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public class ScheduledBotCallback : IJob
  {
    public void Execute(IJobExecutionContext context)
    {
      string output = context.JobDetail.JobDataMap.GetString("Output");
      string botId = context.JobDetail.JobDataMap.GetString("BotId");

      Response response = new Response(new string[] { output }, null); // FIXME: state is needed!
      Bot bot = BotRepository.Get(botId);
      bot.SendResponse(response);
    }
  }
}
