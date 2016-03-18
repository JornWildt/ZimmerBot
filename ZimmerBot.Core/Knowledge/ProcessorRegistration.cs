﻿using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Knowledge
{
  public class ProcessorRegistration
  {
    public string Name { get; protected set; }

    public List<string> RequiredOutputTemplateNames { get; protected set; }

    public Func<ProcessorInput, Func<string>> Processor { get; protected set; }

    public ProcessorRegistration(string name, Func<ProcessorInput, Func<string>> processor)
    {
      Condition.Requires(name, "name").IsNotNullOrEmpty();
      Condition.Requires(processor, "processor").IsNotNull();

      Name = name;
      RequiredOutputTemplateNames = new List<string>();
      Processor = processor;
    }

    public ProcessorRegistration WithRequiredTemplate(string templateName)
    {
      RequiredOutputTemplateNames.Add(templateName);
      return this;
    }
  }
}
