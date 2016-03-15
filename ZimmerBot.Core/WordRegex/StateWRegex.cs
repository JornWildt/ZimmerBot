﻿using CuttingEdge.Conditions;
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


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      if (context.State[Variable] != null)
        return (context.State[Variable].Equals(Value) ? 2 : 0);
      return (context.State[Variable] == Value ? 2 : 0);
    }
  }
}
