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
          SpellChecker.Initialize();
          BackupManager.Initialize();
          TextMerge.Initialize();
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
          SpellChecker.Shutdown();
          CryptoHelper.Shutdown();
          StdSchedulerFactory.GetDefaultScheduler().Shutdown();
          RDFStoreRepository.Shutdown();
          SessionManager.Shutdown();
          AddOnHandling.AddOnLoader.ShutdownAddOns();
          IsInitialized = false;
        }
      }
    }
  }
}
