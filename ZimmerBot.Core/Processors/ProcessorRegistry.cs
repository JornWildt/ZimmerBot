﻿using System;
using System.Collections.Generic;
using log4net;


namespace ZimmerBot.Core.Processors
{
  public static class ProcessorRegistry
  {
    public static ILog Logger = LogManager.GetLogger(typeof(ProcessorRegistry));

    private static Dictionary<string, ProcessorRegistration> Processors = new Dictionary<string, ProcessorRegistration>();


    public static ProcessorRegistration RegisterProcessor(string name, Func<ProcessorInput, ProcessorOutput> f)
    {
      Logger.Debug($"Register processor function '{name}'");
      ProcessorRegistration registration = new ProcessorRegistration(name, f);
      Processors.Add(name, registration);
      return registration;
    }


    public static ProcessorRegistration GetProcessor(string functionName)
    {
      if (!Processors.ContainsKey(functionName))
        throw new ArgumentException($"No processor function named {functionName} found.");
      ProcessorRegistration p = Processors[functionName];
      return p;
    }
  }
}
