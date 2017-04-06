using Quartz.Impl;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.StandardProcessors;
using ZimmerBot.Core.Utilities;

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
          CryptoHelper.Initialize();
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
          CryptoHelper.Shutdown();
          StdSchedulerFactory.GetDefaultScheduler().Shutdown();
          AddOnHandling.AddOnLoader.ShutdownAddOns();
          RDFStoreRepository.Shutdown();
          IsInitialized = false;
        }
      }
    }
  }
}
