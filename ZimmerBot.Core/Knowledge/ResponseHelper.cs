using System;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public static class ResponseHelper
  {
    public static Random Randomizer = new Random();


    public static Invocation OneOf(params object[] choices)
    {
      ProcessorRegistration p = new ProcessorRegistration(
       "oneOf", 
        inp => () => TextMerge.MergeTemplate(choices[Randomizer.Next(choices.Length)].ToString(), inp.Context.Match.Matches));
      Invocation i = new Invocation(p);
      return i;
    }
  }
}
