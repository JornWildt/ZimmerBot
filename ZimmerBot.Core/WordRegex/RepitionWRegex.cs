using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  /// <summary>
  /// Represents zero or more repetisions of a predicate
  /// </summary>
  public class RepitionWRegex : WRegex
  {
    public WRegex A { get; protected set; }


    public RepitionWRegex(WRegex a)
    {
      Condition.Requires(a, "a").IsNotNull();
      A = a;
    }


    public override WRegex GetLookahead()
    {
      return A;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      // Empty sequnce is a match
      if (context.CurrentTokenIndex >= context.Input.Count)
        return 1;

      int currentTokenIndex = context.CurrentTokenIndex;

      while (context.CurrentTokenIndex < context.Input.Count)
      {
        int index = context.CurrentTokenIndex;

        WRegex lookahead2 = lookahead.GetLookahead();
        double lookaheadScore = lookahead.CalculateTriggerScore(context, lookahead2);

        context.CurrentTokenIndex = index;
        double score = A.CalculateTriggerScore(context, lookahead);

        if (score < 1 || lookaheadScore == 1)
        {
          context.CurrentTokenIndex = index;
          break;
        }
      }

      return 1;
    }
  }
}
