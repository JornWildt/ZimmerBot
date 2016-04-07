using System.Collections.Generic;

namespace ZimmerBot.Core.Knowledge
{
  public static class SessionManager
  {
    static Dictionary<string, Session> Sessions { get; set; } = new Dictionary<string, Session>();


    public static Session GetOrCreateSession(string sessionId)
    {
      if (!Sessions.ContainsKey(sessionId))
        Sessions[sessionId] = new Session(sessionId);
      return Sessions[sessionId];
    }
  }
}
