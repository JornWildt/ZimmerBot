using System.Collections.Generic;

namespace ZimmerBot.Core.Utilities
{
  public class NullValueDictionary<TKey,TValue> : Dictionary<TKey, TValue>
    where TValue : new()
  {
    public new TValue this[TKey key]
    {
      get
      {
        if (!ContainsKey(key))
          return default(TValue);
        return base[key];
      }
      set
      {
        base[key] = value;
      }
    }
  }
}
