using System;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Knowledge
{
  public static class ResponseHelper
  {
    public static Random Randomizer = new Random();


    public static CallBinding OneOf(params object[] choices)
    {
      // Fake a processor registration
      ProcessorRegistration p = new ProcessorRegistration(
       "oneOf", 
        inp => () => TextMerge.MergeTemplate(choices[Randomizer.Next(choices.Length)].ToString(), inp.Context.Match.Matches));
      CallBinding i = new CallBinding(p);
      return i;
    }
  }
}
