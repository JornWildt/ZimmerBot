using System;


namespace ZimmerBot.Core.Knowledge
{
  public static class ResponseHelper
  {
    public static Random Randomizer = new Random();


    public static Func<string> OneOf(params object[] choices)
    {
      return () => choices[Randomizer.Next(choices.Length)].ToString();
    }
  }
}
