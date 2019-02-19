using System;
using System.Collections.Generic;
using System.Timers;

namespace ZimmerBot.Core.Knowledge
{
  public static class RDFStoreRepository
  {
    static object BackupLock = new object();

    static List<RDFStore> Stores { get; set; } = new List<RDFStore>();


    public static void Add(RDFStore store)
    {
      Stores.Add(store);
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
  }
}
