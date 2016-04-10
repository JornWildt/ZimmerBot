using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core
{
  public class Response
  {
    public string[] Output { get; set; }

    public object State { get; set; }

    public Response(IEnumerable<string> output, object state)
    {
      Condition.Requires(output, nameof(output)).IsNotNull();

      Output = output.ToArray();
      State = state;
    }
  }
}
