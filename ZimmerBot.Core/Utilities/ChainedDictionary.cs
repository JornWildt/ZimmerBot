using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Utilities
{
  public class ChainedDictionary<K, V> : IDictionary<K, V>, IDictionary
  {
    protected Stack<IDictionary<K, V>> Dictionaries { get; set; }


    public ChainedDictionary()
      : this(new Dictionary<K,V>())
    {
    }


    public ChainedDictionary(IDictionary<K, V> dictionary)
    {
      Condition.Requires(dictionary, nameof(dictionary)).IsNotNull();
      Dictionaries = new Stack<IDictionary<K, V>>();
      Dictionaries.Push(dictionary);
    }


    public void Push(IDictionary<K, V> dictionary)
    {
      Dictionaries.Push(dictionary);
    }


    public IDictionary<K, V> Pop()
    {
      return Dictionaries.Pop();
    }


    public V this[K key]
    {
      get
      {
        foreach (IDictionary<K, V> d in Dictionaries)
          if (d.ContainsKey(key))
            return d[key];
        return default(V);
      }

      set
      {
        Dictionaries.Peek()[key] = value;
      }
    }


    public int Count
    {
      get
      {
        return Dictionaries.Sum(d => d.Count);
      }
    }


    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }


    public ICollection<K> Keys
    {
      get
      {
        List<K> keys = new List<K>();
        foreach (var d in Dictionaries)
          keys.AddRange(d.Keys);
        return keys;
      }
    }


    public ICollection<V> Values
    {
      get
      {
        List<V> values = new List<V>();
        foreach (var d in Dictionaries)
          values.AddRange(d.Values);
        return values;
      }
    }


    public void Add(KeyValuePair<K, V> item)
    {
      Dictionaries.Peek().Add(item);
    }


    public void Add(K key, V value)
    {
      Dictionaries.Peek().Add(key, value);
    }


    public void Clear()
    {
      foreach (var d in Dictionaries)
        d.Clear();
    }


    public bool Contains(KeyValuePair<K, V> item)
    {
      return Dictionaries.Any(d => d.Contains(item));
    }


    public bool ContainsKey(K key)
    {
      return Dictionaries.Any(d => d.ContainsKey(key));
    }


    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }


    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
      throw new NotImplementedException();
    }


    public bool Remove(KeyValuePair<K, V> item)
    {
      foreach (var d in Dictionaries)
        if (d.Remove(item))
          return true;
      return false;
    }


    public bool Remove(K key)
    {
      foreach (var d in Dictionaries)
        if (d.Remove(key))
          return true;
      return false;
    }


    public bool TryGetValue(K key, out V value)
    {
      foreach (var d in Dictionaries)
        if (d.TryGetValue(key, out value))
          return true;
      value = default(V);
      return false;
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }


    #region IDictionary (non generic)

    ICollection IDictionary.Keys
    {
      get
      {
        return (ICollection)Keys;
      }
    }

    ICollection IDictionary.Values
    {
      get
      {
        return (ICollection)Values;
      }
    }

    public bool IsFixedSize
    {
      get
      {
        return false;
      }
    }


    private object _syncRoot = new object();

    public object SyncRoot
    {
      get
      {
        return _syncRoot;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public object this[object key]
    {
      get
      {
        if (key is K)
          return this[(K)key];
        return null;
      }

      set
      {
        if (key is K && value is V)
          this[(K)key] = (V)value;
      }
    }


    public bool Contains(object key)
    {
      if (key is K)
        return ContainsKey((K)key);
      return false;
    }


    public void Add(object key, object value)
    {
      if (key is K && value is V)
        Add((K)key, (V)value);
    }


    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      throw new NotImplementedException();
    }


    public void Remove(object key)
    {
      if (key is K)
        Remove((K)key);
    }


    public void CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    #endregion IDictionary (non generic)
  }
}
