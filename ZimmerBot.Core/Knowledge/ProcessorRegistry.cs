using System;
using System.Collections.Generic;
using log4net;


namespace ZimmerBot.Core.Knowledge
{
  public static class ProcessorRegistry
  {
    public static ILog Logger = LogManager.GetLogger(typeof(ProcessorRegistry));

    private static Dictionary<string, ProcessorRegistration> Processors = new Dictionary<string, ProcessorRegistration>();


    public static ProcessorRegistration RegisterProcessor(string name, Func<ProcessorInput, Func<string>> processor)
    {
      ProcessorRegistration pr = new ProcessorRegistration(name, processor);
      Processors.Add(name, pr);
      return pr;
    }


    public static Invocation Invoke(string functionName, params string[] parameters)
    {
      if (!Processors.ContainsKey(functionName))
        throw new ArgumentException($"No processor function named {functionName} found.");

      ProcessorRegistration p = Processors[functionName];

      return new Invocation(p, parameters);

      //ProcessorInput input = new ProcessorInput(rc);
      //foreach (string p in parameters)
      //{
      //  object value = (rc.Match.Matches.ContainsKey(p) ? rc.Match.Matches[p] : null);
      //  input.Inputs.Add(value);
      //}
      //return Invoke(functionName, input);
    }
  }
}
