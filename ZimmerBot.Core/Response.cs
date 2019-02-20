using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core
{
  public class Response
  {
    public string[] Output { get; protected set; }

    public Dictionary<string,string> State { get; protected set; }

    public Session Session { get; protected set; }

    public Response(IEnumerable<string> output, Dictionary<string, string> state, Session session)
    {
      Condition.Requires(output, nameof(output)).IsNotNull();
      Condition.Requires(session, nameof(session)).IsNotNull();

      Output = output.ToArray();
      State = state;
      Session = session;
    }
  }
}
