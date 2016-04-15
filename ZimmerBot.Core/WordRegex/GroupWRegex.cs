﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class GroupWRegex : WRegexBase
  {
    public WRegexBase Sub { get; protected set; }


    public GroupWRegex(WRegexBase sub)
    {
      Sub = sub;
    }


    public override double CalculateSize()
    {
      return Sub.CalculateSize();
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      string index = (context.CurrentRepetitionIndex++).ToString();

      context.MatchNames.Push(index);
      NFAFragment e = Sub.CalculateNFAFragment(context);
      context.MatchNames.Pop();

      return e;
    }
  }
}
