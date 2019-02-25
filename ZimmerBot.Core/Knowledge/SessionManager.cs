using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZimmerBot.Core.Knowledge
{
  public static class SessionManager
  {
    private static object BackupLock = new object();

    private static ConcurrentDictionary<string, Session> Sessions { get; set; } = new ConcurrentDictionary<string, Session>();


    public static void Initialize(KnowledgeBase.InitializationMode mode)
    {
      if (mode == KnowledgeBase.InitializationMode.Clear)
        Clear();
      else if (mode == KnowledgeBase.InitializationMode.Restore)
        Restore();
      else if (mode == KnowledgeBase.InitializationMode.RestoreIfExists)
        RestoreIfExists();
    }


    private static void Clear()
    {
      string dbFilename = GetDatabaseFilename();
      File.Delete(dbFilename);
    }


    public static void Restore()
    {
      string dbFilename = GetDatabaseFilename();
      using (var input = File.Open(dbFilename, FileMode.Open))
      {
        IFormatter formatter = new BinaryFormatter();
        Sessions = (ConcurrentDictionary<string, Session>)formatter.Deserialize(input);
      }
    }


    private static void RestoreIfExists()
    {
      string dbFilename = GetDatabaseFilename();
      if (File.Exists(dbFilename))
        Restore();
      else
        Clear();
    }


    public static void Shutdown()
    {
      Backup();
    }


    public static void Backup()
    {
      lock (BackupLock)
      {
        string dbFilename = GetDatabaseFilename();
        string lockFilename = dbFilename + ".lock";

        using (new FileStream(lockFilename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
        {
          string backupFilename = dbFilename + ".bak";

          using (var output = File.Create(backupFilename))
          {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(output, Sessions);
          }

          File.Delete(dbFilename);
          File.Move(backupFilename, dbFilename);
        }
      }
    }


    private static string GetDatabaseFilename()
    {
      string filename = Path.Combine(AppSettings.RDF_DataDirectory, "Sessions.dat");
      filename = AppSettings.MapServerPath(filename);
      return filename;
    }


    public static void ClearSessions()
    {
      Sessions.Clear();
    }


    public static Session GetOrCreateSession(string sessionId, string userId)
    {
      if (!Sessions.ContainsKey(sessionId))
        Sessions[sessionId] = new Session(sessionId, userId);
      return Sessions[sessionId];
    }


    public static Session GetSession(string sessionId)
    {
      if (!Sessions.ContainsKey(sessionId))
        throw new InvalidOperationException($"No session with ID '{sessionId}' found.");
      return Sessions[sessionId];
    }


    public static IEnumerable<Session> GetSessions()
    {
      return Sessions.Values;
    }
  }
}
