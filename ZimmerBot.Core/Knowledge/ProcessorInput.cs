using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ProcessorInput
  {
    public List<object> Inputs { get; protected set; }

    public ResponseContext Context { get; protected set; }

    // FIXME: better naming (output template?)
    public string Template { get; protected set; }


    public ProcessorInput(ResponseContext rc, string template)
    {
      Inputs = new List<object>();
      Context = rc;
      Template = template;
    }


    public T GetParameter<T>(int i)
    {
      if (Inputs.Count <= i)
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
