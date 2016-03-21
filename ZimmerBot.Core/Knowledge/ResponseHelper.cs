﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Knowledge
{
  public static class ResponseHelper
  {
    public static Random Randomizer = new Random();


    public static CallBinding OneOf(params string[] choices)
    {
      return OneOf(choices.Cast<string>().ToList());
    }


    public static CallBinding OneOf(IList<string> choices)
    {
      // Fake a processor registration
      ProcessorRegistration p = new ProcessorRegistration(
       "oneOf", 
        inp => TextMerge.MergeTemplate(choices[Randomizer.Next(choices.Count)].ToString(), inp.Context.Match.Matches));
      CallBinding i = new CallBinding(p);
      return i;
    }
  }
}
