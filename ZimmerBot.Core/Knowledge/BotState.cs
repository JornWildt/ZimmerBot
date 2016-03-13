using System.Collections.Generic;


namespace ZimmerBot.Core.Knowledge
{
  public class BotState
  {
    protected Dictionary<string, object> State { get; set; }


    public BotState()
    {
      State = new Dictionary<string, object>();
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


    public object this[string key]
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
