﻿using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Processors
{
  public class ProcessorInput
  {
    public ResponseContext Context { get; protected set; }

    public List<object> Inputs { get; protected set; }


    public ProcessorInput(ResponseContext context, List<object> inputs)
    {
      Condition.Requires(inputs, "inputs").IsNotNull();

      Context = context;
      Inputs = inputs;
    }


    public T GetParameter<T>(int i)
    {
      if (i >= Inputs.Count)
        throw new ArgumentOutOfRangeException($"Input parameter {i} out of range. Got only {Inputs.Count} parameters.");

      object input = Inputs[i];

      if (input == null)
        return default(T);

      if (typeof(T).IsAssignableFrom(input.GetType()))
        return (T)input;

      throw new ArgumentException($"Input parameter {i} was not of the expected type {typeof(T)}. Got {input.GetType()} instead.");
    }
  }
}
