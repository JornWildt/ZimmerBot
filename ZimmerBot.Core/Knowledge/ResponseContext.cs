using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ResponseContext
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public EvaluationContext EvaluationContext { get; protected set; }

    public RequestState State { get { return EvaluationContext.State; } }

    public Request OriginalRequest { get { return EvaluationContext.OriginalRequest; } }

    public ZTokenSequence Input { get { return EvaluationContext.Input; } }

    public WRegex.MatchResult Match { get; protected set; }

    public ChainedDictionary<string, object> Variables { get; protected set; }


    public ResponseContext(KnowledgeBase kb, EvaluationContext context, WRegex.MatchResult match)
    {
      // Both input and match can be null for scheduled, non-input based, responses
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(context, nameof(context)).IsNotNull();

      KnowledgeBase = kb;
      EvaluationContext = context;
      Match = match;

      // Register core bot state in variables
      Variables = new ChainedDictionary<string, object>(State.State);
    }


    public EvaluationContext BuildEvaluationContext()
    {
      return new EvaluationContext(State, OriginalRequest, Input, null, false);
    }


    public ExpressionEvaluationContext BuildExpressionEvaluationContext()
    {
      return new ExpressionEvaluationContext(Variables);
    }


    public void Continue(string input = null)
    {
      EvaluationContext.Continue(input);
    }
  }
}
