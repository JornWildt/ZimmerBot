using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class StateWRegex : WRegex
  {
    protected string Variable { get; set; }

    protected object Value { get; set; }


    // Assume: variable = value
    public StateWRegex(string variable, object value)
    {
      Condition.Requires(variable, "variable").IsNotNullOrEmpty();

      Variable = variable;
      Value = value;
    }


    public static StateWRegex Equals(string variable, object value)
      => new StateWRegex(variable, value);


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
      if (context.State[Variable] != null)
        return (context.State[Variable].Equals(Value) ? new MatchResult(2) : new MatchResult(0));
      return (context.State[Variable] == Value ? new MatchResult(2) : new MatchResult(0));
    }
  }
}
