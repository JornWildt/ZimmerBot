using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public static class ProcessorRegistry
  {
    public class ProcessorInput
    {
      public List<object> Inputs { get; protected set; }

      public ProcessorInput()
      {
        Inputs = new List<object>();
      }

      public ProcessorInput(object p1)
        : this()
      {
        Inputs.Add(p1);
      }
    }


    private class ProcessorRegistration
    {
      public string Name { get; protected set; }

      public Func<ProcessorInput, Func<string>> Processor { get; protected set; }

      public ProcessorRegistration(string name, Func<ProcessorInput, Func<string>> processor)
      {
        Condition.Requires(name, "name").IsNotNullOrEmpty();
        Condition.Requires(processor, "processor").IsNotNull();

        Name = name;
        Processor = processor;
      }
    }


    private static Dictionary<string, ProcessorRegistration> Processors = new Dictionary<string, ProcessorRegistration>();


    public static void AddProcessor(string name, Func<ProcessorInput, Func<string>> processor)
    {
      Processors.Add(name, new ProcessorRegistration(name, processor));
    }


    public static Func<string> Invoke(string name, ProcessorInput input)
    {
      ProcessorRegistration p = Processors[name];
      return p.Processor(input);
    }


    public static Func<string> Invoke(WRegex.MatchResult matches, string functionName, params string[] parameters)
    {
      ProcessorInput input = new ProcessorInput();
      foreach (string p in parameters)
      {
        object value = (matches.Matches.ContainsKey(p) ? matches.Matches[p] : null);
        input.Inputs.Add(value);
      }
      return Invoke(functionName, input);
    }
  }
}
