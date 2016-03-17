using System;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public static class ResponseHelper
  {
    public static Random Randomizer = new Random();


    public static Func<string> OneOf(ResponseContext rc, params object[] choices)
    {
      return () =>
        TextMerge.MergeTemplate(choices[Randomizer.Next(choices.Length)].ToString(), rc.Match.Matches);
    }
  }
}
