using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WordWRegex : WRegex
  {
    public string Word { get; set; }


    public WordWRegex(string w)
      : this(w, null)
    {
    }


    public WordWRegex(string w, string matchName)
    {
      Condition.Requires(w, "w").IsNotNull();
      Word = w;
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
      // This calculation is admittingly not perfect - but it makes some sort of sense ...

      if (context.CurrentTokenIndex < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(1).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex - 1].OriginalText);
        }
      }

      if (context.CurrentTokenIndex + 1 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex + 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.5).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex - 1 >= 0 && context.CurrentTokenIndex - 1 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex - 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.5).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex - 2].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex + 2 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex + 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.25).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex + 1].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex - 2 >= 0 && context.CurrentTokenIndex - 2 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex - 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.25).RegisterMatch(MatchName, context.Input[context.CurrentTokenIndex - 3].OriginalText); ;
        }
      }

      ++context.CurrentTokenIndex;
      return new MatchResult(0.1);
    }
  }
}
