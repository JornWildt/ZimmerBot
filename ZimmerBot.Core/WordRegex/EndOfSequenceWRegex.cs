using System;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class EndOfSequenceWRegex : WRegex
  {
    public override double CalculateSize()
    {
      return 0;
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override MatchResult CalculateMatchResult(TriggerEvaluationContext context, WRegex lookahead)
    {
      return context.CurrentTokenIndex >= context.InputContext.Input.Count
        ? new MatchResult(1, "") 
        : new MatchResult(0, "");
    }
  }
}
