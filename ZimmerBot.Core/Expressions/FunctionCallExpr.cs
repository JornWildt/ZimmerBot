using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Expressions
{
  public class FunctionCallExpr : Expression
  {
    public string FunctionName { get; protected set; }

    public List<Expression> Parameters { get; protected set; }


    public FunctionCallExpr(Expression functionReference, List<Expression> parameters)
    {
      Condition.Requires(functionReference, nameof(functionReference)).IsNotNull();
      Condition.Requires(parameters, nameof(parameters)).IsNotNull();

      FunctionName = functionReference.ToString();
      Parameters = parameters;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      // FIXME: handle through some sort of registration system or maybe even reflection
      if (FunctionName == "probability")
        return Invoke<double>(context, new Type[] { typeof(double) }, p => Probability(p), "probability");
      if (FunctionName == "silent")
        return Invoke<TimeSpan>(context, new Type[] { typeof(TimeSpan) }, p => Silent(context, p), "silent");
      else
        throw new InvalidOperationException(string.Format("Unknown function name '{0}'", FunctionName));
    }


    public override void AssignValue(ExpressionEvaluationContext context, object value)
    {
      throw new ApplicationException("It is not possible to assign to function calls.");
    }


    private object Invoke<T>(ExpressionEvaluationContext context, Type[] types, Func<T,object> f, string name)
    {
      List<object> inputValues = CalculateInputValues(context);
      CheckParameters(types, inputValues, name);
      return f((T)inputValues[0]);
    }


    public List<object> CalculateInputValues(ExpressionEvaluationContext context)
    {
      return Parameters.Select(p => p.Evaluate(context)).ToList();
    }


    public override string ToString()
    {
      return FunctionName + "(" + (Parameters.Select(p=>p.ToString()).Aggregate((a,b) => a+","+b)) + ")";
    }


    private void CheckParameters(Type[] types, List<object> inputValues, string name)
    {
      if (types.Length != inputValues.Count)
        throw new InvalidOperationException(string.Format("Parameter number mismatch for function '{0}'. Got {1} but expected {2}.", 
          name, inputValues.Count, types.Length));

      for (int i = 0; i < inputValues.Count; ++i)
      {
        if (inputValues[i] != null && inputValues[i].GetType() != types[i])
          throw new InvalidOperationException(string.Format("Type mismatch in parameter {0} of {1}. Got {2} but expected {3}.",
            i+1, name, inputValues[i].GetType(), types[i]));
      }
    }


    // FIXME: move to a separate utility class
    static Random Randomizer = new Random();


    private bool Probability(double p)
    {
      return Randomizer.NextDouble() < (double)p;
    }


    private bool Silent(ExpressionEvaluationContext context, TimeSpan t)
    {
      DateTime? last = context.Session.Store[SessionKeys.LastMessageTimeStamp] as DateTime?;
      if (last != null)
        return last.Value < DateTime.Now - t;
      return true;
    }
  }
}
