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
      ProcessorRegistry.RegisterProcessor("RDF.TrySetTopic", TrySetTopic);
    }


    public static ProcessorOutput Query(ProcessorInput input)
    {
      string query = input.GetParameter<string>(0);

      Dictionary<string, object> result = new Dictionary<string, object>();

      RDFResultSet output = input.Context.KnowledgeBase.MemoryStore.Query(
        query,
        input.Context.Match.Matches,
        input.Inputs.GetRange(1, input.Inputs.Count - 1));

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


    public static ProcessorOutput TrySetTopic(ProcessorInput input)
    {
      ProcessorOutput lastValue = input.GetParameter<ProcessorOutput>(0);
      string topicParameterName = input.GetOptionalParameter<string>(1, "topic");

      IDictionary<string, object> output = lastValue?.Value;
      if (output != null)
      {
        if (output.ContainsKey("result"))
        {
          RDFResultSet result = output["result"] as RDFResultSet;

          if (result != null && result.Count > 0 && result[0].ContainsKey(topicParameterName))
          {
            string topic = result[0][topicParameterName] as string;

            if (topic != null && input.Context.KnowledgeBase.Topics.ContainsKey(topic))
            {
              input.Context.Session.SetCurrentTopic(topic);
            }
          }
        }
      }

      return lastValue;
    }
  }
}
