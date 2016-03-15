using System;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WildcardWRegex : WRegex
  {
    public override double CalculateSize()
    {
      return 1;
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      if (context.CurrentTokenIndex < context.Input.Count)
      {
        ++context.CurrentTokenIndex;
        return 1;
      }

      return 0;
    }
  }
}
