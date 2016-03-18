using System;
using System.Collections.Generic;
using log4net;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Processors
{
  public static class ProcessorRegistry
  {
    public static ILog Logger = LogManager.GetLogger(typeof(ProcessorRegistry));

    private static Dictionary<string, ProcessorRegistration> Processors = new Dictionary<string, ProcessorRegistration>();


    public static ProcessorRegistration RegisterProcessor(string name, Func<ProcessorInput, string> processor)
    {
      Logger.Debug($"Register processor function '{name}'");
      ProcessorRegistration pr = new ProcessorRegistration(name, processor);
      Processors.Add(name, pr);
      return pr;
    }


    public static CallBinding BindTo(string functionName, params string[] parameters)
    {
      Logger.Debug($"Bind function '{functionName}' to parameters.");

      if (!Processors.ContainsKey(functionName))
        throw new ArgumentException($"No processor function named {functionName} found.");

      ProcessorRegistration p = Processors[functionName];

      return new CallBinding(p, parameters);
    }
  }
}
