using Quartz.Impl;


namespace ZimmerBot.Core
{
  public class ZimmerBotConfiguration
  {
    public static void Initialize()
    {
      AddOnHandling.AddOnLoader.InitializeAddOns();
      StdSchedulerFactory.GetDefaultScheduler().Start();
    }


    public static void Shutdown()
    {
      StdSchedulerFactory.GetDefaultScheduler().Shutdown();
      AddOnHandling.AddOnLoader.ShutdownAddOns();
    }
  }
}
