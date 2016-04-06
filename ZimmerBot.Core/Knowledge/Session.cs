using System.Collections.Generic;
using System.Collections.Specialized;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class Session
  {
    public SessionState State { get; protected set; }


    public Session()
    {
      State = new SessionState();

      // HybridDictionary => return null on missing keys instead of throwing exceptions
      var sessionStore = new NullValueDictionary<string, object>();
      sessionStore[Constants.LineCountKey] = 0d;
      State[Constants.SessionStoreKey] = sessionStore;
    }
  }
}
