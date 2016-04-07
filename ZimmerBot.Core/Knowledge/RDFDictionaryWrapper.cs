using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using VDS.RDF;

namespace ZimmerBot.Core.Knowledge
{
  public class RDFDictionaryWrapper : IDictionary<string, object>
  {
    Dictionary<string, object> Cache = new Dictionary<string, object>();

    protected RDFStore Store { get; set; }

    protected NodeFactory NodeFactory { get; set; }

    protected Uri Subject { get; set; }

    protected Uri PredicateBase { get; set; }


    public RDFDictionaryWrapper(RDFStore store, Uri subject, Uri predicateBase)
    {
      Condition.Requires(store, nameof(store)).IsNotNull();
      Condition.Requires(subject, nameof(subject)).IsNotNull();
      Condition.Requires(predicateBase, nameof(predicateBase)).IsNotNull();

      Store = store;
      Subject = subject;
      PredicateBase = predicateBase;
      NodeFactory = new NodeFactory();
    }


    public object this[string key]
    {
      get
      {
        if (!Cache.ContainsKey(key))
        {
          INode s = NodeFactory.CreateUriNode(Subject);
          INode p = NodeFactory.CreateUriNode(new Uri(PredicateBase, key));
          Triple t = Store.GetTripple(s, p);
          if (t != null)
            Cache[key] = t.Object.ToString();
          else
            Cache[key] = null;
        }

        return Cache[key];
      }

      set
      {
        Cache[key] = value;

        INode s = NodeFactory.CreateUriNode(Subject);
        INode p = NodeFactory.CreateUriNode(new Uri(PredicateBase, key));
        INode o = NodeFactory.CreateLiteralNode(value != null ? value.ToString() : "");
        Store.Retract(s, p);
        Store.Insert(s, p, o);
      }
    }

    public int Count
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public ICollection<string> Keys
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ICollection<object> Values
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Add(KeyValuePair<string, object> item)
    {
      Cache[item.Key] = item.Value;
    }

    public void Add(string key, object value)
    {
      Cache[key] = value;
    }

    public void Clear()
    {
      Cache.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    public bool ContainsKey(string key)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    public bool Remove(string key)
    {
      throw new NotImplementedException();
    }

    public bool TryGetValue(string key, out object value)
    {
      return Cache.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}
