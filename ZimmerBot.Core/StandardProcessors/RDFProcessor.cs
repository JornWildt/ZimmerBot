using System.Collections.Generic;
using ZimmerBot.Core.Processors;

namespace ZimmerBot.Core.StandardProcessors
{
  public static class RDFProcessor
  {
    public static void Initialize()
    {
      ProcessorRegistry.RegisterProcessor("RDF.Query", Query);
    }


    public static ProcessorOutput Query(ProcessorInput input)
    {
      string query = input.GetParameter<string>(0);

      //input.Context.State.
      object output = "HEJ";

      Dictionary<string, object> result = new Dictionary<string, object>();
      result["result"] = output;
      return new ProcessorOutput(result);
    }
  }
}
