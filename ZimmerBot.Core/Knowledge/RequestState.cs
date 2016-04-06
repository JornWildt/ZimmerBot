using System.Collections.Generic;


namespace ZimmerBot.Core.Knowledge
{
  public class RequestState
  {
    public Dictionary<string, dynamic> State { get; protected set; }


    public RequestState()
    {
      State = new Dictionary<string, dynamic>();
    }


    public void Store(string key, object value)
    {
      State[key] = value;
    }


    public object Get(string key)
    {
      if (!State.ContainsKey(key))
        return null;
      return State[key];
    }


    public dynamic this[string key]
    {
      set
      {
        Store(key, value);
      }
      get
      {
        return Get(key);
      }
    }
  }
}
