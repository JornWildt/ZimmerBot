using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ResponseContext
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public RequestState State { get; protected set; }

    public Request OriginalRequest { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public WRegex.MatchResult Match { get; protected set; }

    public ChainedDictionary<string, object> Variables { get; protected set; }


    public ResponseContext(KnowledgeBase kb, RequestState state, Request originalRequest, ZTokenSequence input, WRegex.MatchResult match)
    {
      // Both input and match can be null for scheduled, non-input based, responses
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(originalRequest, nameof(originalRequest)).IsNotNull();

      KnowledgeBase = kb;
      State = state;
      OriginalRequest = originalRequest;
      Input = input;
      Match = match;

      // Register core bot state in variables
      Variables = new ChainedDictionary<string, object>(State.State);
    }
  }
}
