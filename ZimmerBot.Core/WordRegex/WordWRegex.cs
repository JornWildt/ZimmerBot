using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WordWRegex : WRegex
  {
    public string Word { get; set; }


    public WordWRegex(string w)
    {
      Condition.Requires(w, "w").IsNotNull();
      Word = w;
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      // This calculation is admittingly not perfect - but it makes some sort of sense ...

      if (context.CurrentTokenIndex < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return 1;
        }
      }

      if (context.CurrentTokenIndex + 1 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex + 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return 0.5;
        }
      }

      if (context.CurrentTokenIndex - 1 >= 0 && context.CurrentTokenIndex - 1 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex - 1].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return 0.5;
        }
      }

      if (context.CurrentTokenIndex + 2 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex + 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return 0.25;
        }
      }

      if (context.CurrentTokenIndex - 2 >= 0 && context.CurrentTokenIndex - 2 < context.Input.Count)
      {
        if (context.Input[context.CurrentTokenIndex - 2].Matches(Word))
        {
          ++context.CurrentTokenIndex;
          return 0.25;
        }
      }

      ++context.CurrentTokenIndex;
      return 0.1;
    }
  }
}
