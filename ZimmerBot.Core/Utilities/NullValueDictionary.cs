using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZimmerBot.Core.Utilities
{
  public class NullValueDictionary<TKey,TValue> : ConcurrentDictionary<TKey, TValue>, IDictionary<TKey, TValue>
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

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
      get
      {
        return this[key];
      }
      set
      {
        this[key] = value;
      }
    }
  }
}
