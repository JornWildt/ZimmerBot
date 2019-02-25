using CuttingEdge.Conditions;
using System;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  [Serializable]
  public class Session
  {
    public string SessionId { get; protected set; }

    public NullValueDictionary<string,dynamic> Store { get; protected set; }


    public Session(string id, string userId)
    {
      Condition.Requires(id, nameof(id)).IsNotNullOrEmpty();
      Condition.Requires(userId, nameof(userId)).IsNotNullOrEmpty();

      SessionId = id;
      Store = new NullValueDictionary<string, object>();

      Store[SessionKeys.UserId] = userId;
      Store[SessionKeys.ResponseCount] = 0;

      this.MarkAsReadyForInput();
    }
  }
}
