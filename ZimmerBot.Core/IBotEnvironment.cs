using System;


namespace ZimmerBot.Core
{
  public enum LogLevel { Error, Warn, Info, Debug }


  public interface IBotEnvironment
  {
    void HandleResponse(Response response);

    void Log(LogLevel level, string msg, Exception ex);

    void Log(LogLevel level, string msg, params object[] args);
  }
}
