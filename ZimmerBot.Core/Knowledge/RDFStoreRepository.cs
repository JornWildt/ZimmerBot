using System;
using System.Collections.Generic;
using System.Timers;

namespace ZimmerBot.Core.Knowledge
{
  public static class RDFStoreRepository
  {
    static object BackupLock = new object();

    static List<RDFStore> Stores { get; set; } = new List<RDFStore>();

    static Timer BackupTimer { get; set; }


    public static void Add(RDFStore store)
    {
      Stores.Add(store);

      if (BackupTimer == null)
        InitializeBackgroundBackup();
    }


    public static IEnumerable<RDFStore> GetStores()
    {
      return Stores;
    }


    public static void Shutdown()
    {
      Backup();
    }


    public static void Backup()
    {
      lock (BackupLock)
      {
        foreach (var store in Stores)
        {
          store.FlushToDisk();
        }
      }
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
      Backup();
    }
  }
}
