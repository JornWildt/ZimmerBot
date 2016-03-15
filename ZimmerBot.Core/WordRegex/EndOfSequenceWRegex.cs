using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class EndOfSequenceWRegex : WRegex
  {
    public override WRegex GetLookahead()
    {
      return this;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      return context.CurrentTokenIndex >= context.Input.Count ? 1 : 0;
    }
  }
}
