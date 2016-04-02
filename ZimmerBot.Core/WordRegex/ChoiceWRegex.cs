using System.Linq;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class ChoiceWRegex : WRegex
  {
    public List<WRegex> Choices { get; protected set; }


    public ChoiceWRegex()
      : this(Enumerable.Empty<WRegex>())
    {
    }


    public ChoiceWRegex(WRegex left, WRegex right)
      : this(left, right, null)
    {
    }

    public ChoiceWRegex(WRegex left, WRegex right, string matchName)
    {
      Condition.Requires(left, "left").IsNotNull();
      Condition.Requires(right, "right").IsNotNull();

      Choices = new List<WRegex>();
      Choices.Add(left);
      Choices.Add(right);
      MatchName = matchName;
    }


    public ChoiceWRegex(IEnumerable<WRegex> choices)
    {
      Condition.Requires(choices, nameof(choices)).IsNotNull();
      Choices = new List<WRegex>(choices);
    }


    public override double CalculateSize()
    {
      return Choices.Max(c => c.CalculateSize());
    }


    public override WRegex GetLookahead()
    {
      return new ChoiceWRegex(Choices.Select(c => c.GetLookahead()));
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      if (Choices.Count == 0)
        return new MatchResult(0, "");

      MatchResult bestResult = null;
      int bestIndex = -1;

      int initialIndex = context.CurrentTokenIndex;

      for (int i = 0; i < Choices.Count; ++i)
      {
        context.CurrentTokenIndex = initialIndex;
        MatchResult result = Choices[i].CalculateMatchResult(context, lookahead);

        if (bestResult == null  || result.Score >= bestResult.Score)
        {
          bestResult = result;
          bestIndex = context.CurrentTokenIndex;
        }
      }

      context.CurrentTokenIndex = bestIndex;
      return bestResult.RegisterMatch(MatchName, bestResult.MatchedText);
    }
  }
}
