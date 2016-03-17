using System;
using CuttingEdge.Conditions;
using log4net;


namespace ZimmerBot.Core
{
  /// <summary>
  /// A bot environment for interaction through the console.
  /// </summary>
  public class ConsoleBotEnvironment : IBotEnvironment
  {
    static ILog Logger = LogManager.GetLogger(typeof(Bot));


    public string Prompt { get; set; }


    public ConsoleBotEnvironment(string prompt)
    {
      Condition.Requires(prompt, "prompt").IsNotNull();
      Prompt = prompt;
    }


    public void HandleResponse(Response response)
    {
      string[] output = response.Output;

      if (output.Length > 0)
      {
        // Move cursor to the left if anything is on the line already
        if (Console.CursorLeft > 1)
          Console.WriteLine("");

        foreach (string s in output)
          Console.WriteLine(Prompt + s);

        // Prompt ready for user
        Console.Write("> ");
      }
    }


    public void Log(LogLevel level, string msg, Exception ex)
    {
      switch (level)
      {
        case LogLevel.Debug:
          Logger.Debug(msg, ex);
          break;
        case LogLevel.Info:
          Logger.Info(msg, ex);
          break;
        case LogLevel.Warn:
          Logger.Warn(msg, ex);
          break;
        case LogLevel.Error:
          Console.WriteLine("Internal error");
          Logger.Error(msg, ex);
          break;
      }
    }


    public void Log(LogLevel level, string msg, params object[] args)
    {
      switch (level)
      {
        case LogLevel.Debug:
          Logger.DebugFormat(msg, args);
          break;
        case LogLevel.Info:
          Logger.InfoFormat(msg, args);
          break;
        case LogLevel.Warn:
          Logger.WarnFormat(msg, args);
          break;
        case LogLevel.Error:
          Console.WriteLine("Internal error");
          Logger.ErrorFormat(msg, args);
          break;
      }
    }


    public static void RunInteractiveConsoleBot(string prompt, Bot b, Func<string, string> inputModifier = null)
    {
      BotHandle bh = b.Run(new ConsoleBotEnvironment(prompt));

      string input;
      Console.Write("> ");

      do
      {
        input = Console.ReadLine();

        if (inputModifier != null)
          input = inputModifier(input);

        if (!string.IsNullOrEmpty(input))
        {
          bh.Invoke(new Request { Input = input });
        }
      }
      while (!string.IsNullOrEmpty(input));

      bh.Shutdown();
    }
  }
}
