using System;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WildcardWRegex : WRegex
  {
    public WildcardWRegex()
      : this(null)
    {
    }


    public WildcardWRegex(string matchName)
    {
      MatchName = matchName;
    }


    public override double CalculateSize()
    {
      return 1;
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      if (context.CurrentTokenIndex < context.Input.Count)
      {
        ++context.CurrentTokenIndex;
        return new MatchResult(1).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex - 1].OriginalText);
      }

      return new MatchResult(0);
    }
  }
}
