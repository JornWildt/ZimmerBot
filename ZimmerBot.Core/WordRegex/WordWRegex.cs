using System;
using System.Collections.Generic;
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


    public override MatchResult CalculateMatchResult(TriggerEvaluationContext context, WRegex lookahead)
    {
      if (context.CurrentTokenIndex < context.InputContext.Input.Count)
      {
        if (context.InputContext.Input[context.CurrentTokenIndex].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(1, context.InputContext.Input[context.CurrentTokenIndex - 1].OriginalText)
            .RegisterMatch(MatchName, context.InputContext.Input[context.CurrentTokenIndex - 1].OriginalText);
        }
      }

      // Wondering if this really belongs here, outside of the bot author's reach ...
#if true

      if (context.CurrentTokenIndex + 1 < context.InputContext.Input.Count)
      {
        if (context.InputContext.Input[context.CurrentTokenIndex + 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.5, context.InputContext.Input[context.CurrentTokenIndex].OriginalText)
            .RegisterMatch(MatchName, context.InputContext.Input[context.CurrentTokenIndex].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex - 1 >= 0 && context.CurrentTokenIndex - 1 < context.InputContext.Input.Count)
      {
        if (context.InputContext.Input[context.CurrentTokenIndex - 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.5, context.InputContext.Input[context.CurrentTokenIndex - 2].OriginalText)
            .RegisterMatch(MatchName, context.InputContext.Input[context.CurrentTokenIndex - 2].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex + 2 < context.InputContext.Input.Count)
      {
        if (context.InputContext.Input[context.CurrentTokenIndex + 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.25, context.InputContext.Input[context.CurrentTokenIndex + 1].OriginalText)
            .RegisterMatch(MatchName, context.InputContext.Input[context.CurrentTokenIndex + 1].OriginalText); ;
        }
      }

      if (context.CurrentTokenIndex - 2 >= 0 && context.CurrentTokenIndex - 2 < context.InputContext.Input.Count)
      {
        if (context.InputContext.Input[context.CurrentTokenIndex - 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return new MatchResult(0.25, context.InputContext.Input[context.CurrentTokenIndex - 3].OriginalText)
            .RegisterMatch(MatchName, context.InputContext.Input[context.CurrentTokenIndex - 3].OriginalText); ;
        }
      }
#endif

      ++context.CurrentTokenIndex;
      return new MatchResult(0.1, "");
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFANode node = NFANode.CreateLiteral(context, Word);
      return new NFAFragment(node, node.Out);
    }
  }
}
