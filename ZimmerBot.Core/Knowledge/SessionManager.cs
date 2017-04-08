using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZimmerBot.Core.Knowledge
{
  public static class SessionManager
  {
    static ConcurrentDictionary<string, Session> Sessions { get; set; } = new ConcurrentDictionary<string, Session>();


    public static void ClearSessions()
    {
      Sessions.Clear();
    }


    public static Session GetOrCreateSession(string sessionId)
    {
      if (!Sessions.ContainsKey(sessionId))
        Sessions[sessionId] = new Session(sessionId);
      return Sessions[sessionId];
    }


    public static Session GetSession(string sessionId)
    {
      if (!Sessions.ContainsKey(sessionId))
        throw new InvalidOperationException($"No session with ID '{sessionId}' found.");
      return Sessions[sessionId];
    }
  }
}
