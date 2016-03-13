using System;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core
{
  public class ConsoleBotEnvironment : IBotEnvironment
  {
    public string Prompt { get; set; }


    public ConsoleBotEnvironment(string prompt)
    {
      Condition.Requires(prompt, "prompt").IsNotNull();
      Prompt = prompt;
    }


    public void HandleResponse(Response response)
    {
      string[] output = response.Output;

      // Move cursor to the left if anything is on the line already
      if (Console.CursorLeft > 1)
        Console.WriteLine("");

      foreach (string s in output)
        Console.WriteLine(Prompt + s);

      // Prompt ready for user
      Console.Write("> ");
    }


    public void Log(LogLevel level, string msg, Exception ex)
    {
      Console.WriteLine("LOG: {0} - {1}", level, msg);
    }


    public void Log(LogLevel level, string msg, params object[] args)
    {
      Console.WriteLine("LOG: {0} - {1}", level, string.Format(msg, args));
    }
  }
}
