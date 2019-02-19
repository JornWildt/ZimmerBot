using System.Timers;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public static class BackupManager
  {
    private static Timer BackupTimer { get; set; }


    public static void Initialize()
    {
      InitializeBackgroundBackup();
    }


    private static void InitializeBackgroundBackup()
    {
      BackupTimer = new Timer();
      BackupTimer.AutoReset = true;
      BackupTimer.Elapsed += BackupTimer_Elapsed; ;
      BackupTimer.Interval = AppSettings.RDF_BackupInterval.Value.TotalMilliseconds;
      BackupTimer.Start();
    }


    private static void BackupTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      ErrorHandler.Execute(
        () => RDFStoreRepository.Backup(),
        () => SessionManager.Backup());
    }
  }
}
