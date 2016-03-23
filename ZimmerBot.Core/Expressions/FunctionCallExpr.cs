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


    public FunctionCallExpr(string functionName, params Expression[] parameters)
      : this(functionName, parameters.ToList())
    {
    }


    public FunctionCallExpr(string functionName, List<Expression> parameters)
    {
      Condition.Requires(functionName, nameof(functionName)).IsNotNullOrEmpty();
      Condition.Requires(parameters, nameof(parameters)).IsNotNull();

      FunctionName = functionName;
      Parameters = parameters;
    }


    public override object Evaluate(ExpressionEvaluationContext context)
    {
      // FIXME: handle through some sort of registration system or maybe even reflection
      if (FunctionName == "probability")
        return Invoke1(context, new Type[] { typeof(double) }, p => Probability(p), "probability");
      else
        throw new InvalidOperationException(string.Format("Unknown function name '{0}'", FunctionName));
    }


    private object Invoke1(ExpressionEvaluationContext context, Type[] types, Func<object,object> f, string name)
    {
      List<object> inputValues = CalculateInputValues(context);
      CheckParameters(types, inputValues, name);
      return f(inputValues[0]);
    }


    public List<object> CalculateInputValues(ExpressionEvaluationContext context)
    {
      return Parameters.Select(p => p.Evaluate(context)).ToList();
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

    private bool Probability(object p)
    {
      return Randomizer.NextDouble() < (double)p;
    }
  }
}
