using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class InputPipelineItem
  {
    #region Input

    public KnowledgeBase KnowledgeBase { get; protected set; }

    public Session Session { get; protected set; }

    public EvaluationContext EvaluationContext { get; protected set; }

    public Request Request { get { return EvaluationContext.OriginalRequest; } }

    public ZTokenSequence Input { get { return EvaluationContext.Input; } }

    public RequestState State { get { return EvaluationContext.State; } }

    public bool FromTemplate { get; protected set; }

    #endregion


    #region Output

    public HashSet<string> SentenceTags { get; protected set; }

    public ReactionSet Reactions { get; protected set; }

    public List<string> Output { get; protected set; }

    #endregion


    public InputPipelineItem(
      KnowledgeBase kb, 
      Session session, 
      RequestState state, 
      Request req, 
      ZTokenSequence input, 
      bool fromTemplate)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(session, nameof(session)).IsNotNull();
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(req, nameof(req)).IsNotNull();

      KnowledgeBase = kb;
      Session = session;
      EvaluationContext = new EvaluationContext(state, session, req, input, req.RuleId, executeScheduledRules: false);
      FromTemplate = fromTemplate;

      SentenceTags = new HashSet<string>();
      Reactions = new ReactionSet();
      Output = new List<string>();
    }
  }
}
