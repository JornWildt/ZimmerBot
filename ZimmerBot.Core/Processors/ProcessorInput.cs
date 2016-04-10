using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Processors
{
  public class ProcessorInput
  {
    public ResponseGenerationContext Context { get; protected set; }

    public List<object> Inputs { get; protected set; }


    public ProcessorInput(ResponseGenerationContext context, List<object> inputs)
    {
      Condition.Requires(inputs, "inputs").IsNotNull();

      Context = context;
      Inputs = inputs;
    }


    public T GetOptionalParameter<T>(int i, T defaultValue)
    {
      return GetParameter<T>(i, defaultValue: defaultValue, optional: true);
    }


    public T GetParameter<T>(int i, T defaultValue = default(T), bool optional = false)
    {
      if (i >= Inputs.Count)
      {
        if (optional)
          return defaultValue;
        else
          throw new ArgumentOutOfRangeException($"Input parameter {i} out of range. Got only {Inputs.Count} parameters.");
      }

      object input = Inputs[i];

      if (input == null)
        return default(T);

      if (typeof(T).IsAssignableFrom(input.GetType()))
        return (T)input;

      throw new ArgumentException($"Input parameter {i} was not of the expected type {typeof(T)}. Got {input.GetType()} instead.");
    }
  }
}
