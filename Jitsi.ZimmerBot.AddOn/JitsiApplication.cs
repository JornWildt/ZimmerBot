﻿using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Processors;

namespace Jitsi.ZimmerBot.AddOn
{
  public class JitsiApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in scripts
      ProcessorRegistry.RegisterProcessor("Jitsi.Meeting", JitsiProcessor.Meeting);
    }


    public void Shutdown()
    {
    }
  }
}
