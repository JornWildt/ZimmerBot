using System.Linq;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using System;

namespace ZimmerBot.Core.WordRegex
{
  public class WordChoiceWRegex : WRegex
  {
    public HashSet<string> Choices { get; protected set; }


    public WordChoiceWRegex()
      : this(Enumerable.Empty<string>())
    {
    }


    public WordChoiceWRegex(IEnumerable<string> choices)
    {
      Condition.Requires(choices, nameof(choices)).IsNotNull();
      Choices = new HashSet<string>(choices, StringComparer.CurrentCultureIgnoreCase);
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
      ++context.CurrentTokenIndex;

      if (Choices.Contains(context.Input[context.CurrentTokenIndex - 1].OriginalText))
      {
        return new MatchResult(1, context.Input[context.CurrentTokenIndex - 1].OriginalText)
          .RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex - 1].OriginalText);
      }

      return new MatchResult(0.1, "");
    }
  }
}
