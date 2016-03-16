using Quartz.Impl;


namespace ZimmerBot.Core
{
  public class ZimmerBotConfiguration
  {
    public static void Initialize()
    {
      StdSchedulerFactory.GetDefaultScheduler().Start();
    }


    public static void Shutdown()
    {
      StdSchedulerFactory.GetDefaultScheduler().Shutdown();
    }
  }
}
