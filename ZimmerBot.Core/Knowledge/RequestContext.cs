using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  /// <summary>
  /// This class contains various global parameters for invoking the chat system.
  /// </summary>
  public class RequestContext
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public RequestState State { get; protected set; }

    public Session Session { get; protected set; }

    public ChainedDictionary<string, object> Variables { get; protected set; }

    public int RepetitionCount { get; set; }


    public RequestContext(KnowledgeBase kb, RequestState state, Session session)//, Request originalRequest)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(session, nameof(session)).IsNotNull();

      KnowledgeBase = kb;
      State = state;
      Session = session;

      Variables = new ChainedDictionary<string, object>(State.State);
      RepetitionCount = 0;
    }
  }
}
