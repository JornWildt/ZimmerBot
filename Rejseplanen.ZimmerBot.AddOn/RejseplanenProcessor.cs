using System;
using ZimmerBot.Core.Knowledge;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenProcessor
  {
    public static Func<string> FindStation(ProcessorRegistry.ProcessorInput input)
    {
      return () => "STATION: " + input.Inputs[0];
    }
  }
}
