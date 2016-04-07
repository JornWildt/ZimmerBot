using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class Session
  {
    public string SessionId { get; protected set; }

    public NullValueDictionary<string,dynamic> Store { get; protected set; }


    public Session(string id)
    {
      Condition.Requires(id, nameof(id)).IsNotNullOrEmpty();

      SessionId = id;
      Store = new NullValueDictionary<string, object>();

      Store[StateKeys.ResponseCount] = 0;
    }
  }
}
