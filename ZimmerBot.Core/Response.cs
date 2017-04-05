using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core
{
  public class Response
  {
    public string[] Output { get; set; }

    public Dictionary<string,string> State { get; set; }

    public Response(IEnumerable<string> output, Dictionary<string, string> state)
    {
      Condition.Requires(output, nameof(output)).IsNotNull();

      Output = output.ToArray();
      State = state;
    }
  }
}
