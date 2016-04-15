﻿using System;
using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WildcardWRegex : WRegexBase
  {
    public WildcardWRegex()
    {
    }


    public override double CalculateSize()
    {
      return 1;
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFANode node = NFANode.CreateLiteral(context, null);
      return new NFAFragment(node, node.Out);
    }
  }
}
