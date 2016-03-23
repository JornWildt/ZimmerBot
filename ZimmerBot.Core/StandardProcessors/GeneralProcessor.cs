using System.Collections.Generic;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.StandardProcessors
{
  public static class GeneralProcessor
  {
    public static void Initialize()
    {
      ProcessorRegistry.RegisterProcessor("General.Echo", Echo);
    }


    public static ProcessorOutput Echo(ProcessorInput input)
    {
      string text = input.GetParameter<string>(0);
      Dictionary<string, object> result = new Dictionary<string, object>();
      result["result"] = text;
      return new ProcessorOutput(result);
    }
  }
}
