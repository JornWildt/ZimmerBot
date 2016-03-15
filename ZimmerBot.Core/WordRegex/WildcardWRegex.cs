using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WildcardWRegex : WRegex
  {
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
