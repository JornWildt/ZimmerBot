using Quartz.Impl;
using ZimmerBot.Core.StandardProcessors;

namespace ZimmerBot.Core
{
  public class ZimmerBotConfiguration
  {
    private static object LockObject = new object();
    private static bool IsInitialized = false;


    public static void Initialize()
    {
      lock (LockObject)
      {
        if (!IsInitialized)
        {
          AddOnHandling.AddOnLoader.InitializeAddOns();
          GeneralProcessor.Initialize();
          DateTimeProcessor.Initialize();
          RDFProcessor.Initialize();
          StdSchedulerFactory.GetDefaultScheduler().Start();
          IsInitialized = true;
        }
      }
    }


    public static void Shutdown()
    {
      lock (LockObject)
      {
        if (IsInitialized)
        {
          StdSchedulerFactory.GetDefaultScheduler().Shutdown();
          AddOnHandling.AddOnLoader.ShutdownAddOns();
          IsInitialized = false;
        }
      }
    }
  }
}
