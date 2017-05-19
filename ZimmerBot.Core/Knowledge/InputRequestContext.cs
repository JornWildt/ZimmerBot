using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class InputRequestContext
  {
    public RequestContext RequestContext { get; protected set; }

    public KnowledgeBase KnowledgeBase { get { return RequestContext.KnowledgeBase; } }

    public ChainedDictionary<string, object> Variables { get { return RequestContext.Variables; } }

    public RequestState State { get { return RequestContext.State; } }

    public Session Session { get { return RequestContext.Session; } }

    public int RepetitionCount
    {
      get { return RequestContext.RepetitionCount; }
      set { RequestContext.RepetitionCount = value; } 
    }

    public Request Request { get; protected set; }

    public ZTokenSequenceList Input { get; set; }

    public bool FromTemplate { get; protected set; }

    public Continuation ContinuationChoice { get; protected set; }


    public InputRequestContext(RequestContext context, Request request, ZTokenSequence input)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();
      Condition.Requires(request, nameof(request)).IsNotNull();

      RequestContext = context;
      Request = request;
      if (input != null)
        Input = new ZTokenSequenceList { input };
      else
        Input = null;
    }


    public void Continue(Continuation c)
    {
      ContinuationChoice = c;
    }
  }
}
