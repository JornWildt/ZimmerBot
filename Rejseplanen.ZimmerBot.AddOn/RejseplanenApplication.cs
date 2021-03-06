﻿using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Processors;


namespace Rejseplanen.ZimmerBot.AddOn
{
  /// <summary>
  /// This class is instantiated automatically by ZimmerBot on startup/shutdown and handles the corresponding operations.
  /// </summary>
  public class RejseplanenApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in scripts
      ProcessorRegistry
        .RegisterProcessor("Rejseplanen.FindStoppested", RejseplanenProcessor.FindStop)
        .WithRequiredTemplate("default")
        .WithRequiredTemplate("empty")
        .WithRequiredTemplate("error");

      ProcessorRegistry
        .RegisterProcessor("Rejseplanen.FindNextDepartures", RejseplanenProcessor.FindNextDepartures);
    }


    public void Shutdown()
    {
    }
  }
}
