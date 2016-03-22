using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;


namespace ZimmerBot.Core.Processors
{
  public class ProcessorRegistration
  {
    public string Name { get; protected set; }

    public List<string> RequiredOutputTemplateNames { get; protected set; }

    public Func<ProcessorInput, object> Function { get; protected set; }


    public ProcessorRegistration(string name, Func<ProcessorInput, object> f)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty();
      Condition.Requires(f, nameof(f)).IsNotNull();

      Name = name;
      RequiredOutputTemplateNames = new List<string>();
      Function = f;
    }


    public ProcessorRegistration WithRequiredTemplate(string templateName)
    {
      RequiredOutputTemplateNames.Add(templateName);
      return this;
    }
  }
}
