using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Knowledge
{
  public class StatePredicate
  {
    protected string Variable { get; set; }

    protected object Value { get; set; }


    // Assume: variable = value
    public StatePredicate(string variable, object value)
    {
      Condition.Requires(variable, "variable").IsNotNullOrEmpty();

      Variable = variable;
      Value = value;
    }


    public static StatePredicate Equals(string variable, object value)
      => new StatePredicate(variable, value);

    public double CalculateTriggerScore(EvaluationContext context)
    {
      if (context.State[Variable] != null)
        return (context.State[Variable].Equals(Value) ? 2 : 0);
      return (context.State[Variable] == Value ? 2 : 0);
    }
  }
}
