using System.Collections.Generic;

namespace ZimmerBot.Core.Knowledge
{
  public static class RDFStoreRepository
  {
    static List<RDFStore> Stores { get; set; } = new List<RDFStore>();


    public static void Add(RDFStore store)
    {
      Stores.Add(store);
    }


    public static IEnumerable<RDFStore> GetStores()
    {
      return Stores;
    }
  }
}
