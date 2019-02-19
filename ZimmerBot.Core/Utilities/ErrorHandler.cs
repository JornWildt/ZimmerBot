using log4net;
using System;

namespace ZimmerBot.Core.Utilities
{
  public static class ErrorHandler
  {
    private static ILog Logger = LogManager.GetLogger(typeof(ErrorHandler));


    /// <summary>
    /// Execute multiple actions one by one, even if one throws an exception.
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static void Execute(params Action[] actions)
    {
      Exception error = null;
      foreach (var action in actions)
      {
        try
        {
          action();
        }
        catch (Exception ex)
        {
          Logger.Error(ex);
          error = ex;
        }
      }

      if (error != null)
        throw error;
    }
  }
}
