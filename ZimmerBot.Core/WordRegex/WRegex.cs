using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public abstract class WRegex
  {
    public abstract WRegex GetLookahead();

    public abstract double CalculateTriggerScore(EvaluationContext context, WRegex lookahead);
  }
}
