using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public static class ProcessorRegistry
  {
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


    public static Func<string> Invoke(ResponseContext rc, string template, string functionName, params string[] parameters)
    {
      ProcessorInput input = new ProcessorInput(rc, template);
      foreach (string p in parameters)
      {
        object value = (rc.Match.Matches.ContainsKey(p) ? rc.Match.Matches[p] : null);
        input.Inputs.Add(value);
      }
      return Invoke(functionName, input);
    }
  }
}
