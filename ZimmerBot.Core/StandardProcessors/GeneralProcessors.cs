using System.Collections.Generic;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.StandardProcessors
{
  public static class GeneralProcessors
  {
    public static void Initialize()
    {
      ProcessorRegistry.RegisterProcessor("General.Echo", Echo);
    }


    public static object Echo(ProcessorInput input)
    {
      string text = input.GetParameter<string>(0);
      Dictionary<string, object> result = new Dictionary<string, object>();
      result["result"] = text;
      return result;
    }
  }
}
