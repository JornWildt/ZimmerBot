using System.Collections.Generic;

namespace ZimmerBot.Core.Knowledge
{
  public class Session
  {
    public SessionState State { get; protected set; }


    public Session()
    {
      State = new SessionState();

      var sessionStore = new Dictionary<string, object>();
      sessionStore[Constants.LineCountKey] = 0d;
      State[Constants.SessionStoreKey] = sessionStore;
    }
  }
}
