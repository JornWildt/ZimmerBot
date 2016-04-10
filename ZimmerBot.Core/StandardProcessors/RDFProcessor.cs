using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;
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

      Dictionary<string, object> result = new Dictionary<string, object>();

      RDFResultSet output = input.Context.KnowledgeBase.MemoryStore.Query(
        query, 
        input.Context.Match.Matches, 
        input.Inputs.GetRange(1, input.Inputs.Count-1));

      if (output.Count > 0)
      {
        result["result"] = output;
        return new ProcessorOutput(result);
      }
      else
      {
        return new ProcessorOutput("empty", result);
      }
    }
  }
}
