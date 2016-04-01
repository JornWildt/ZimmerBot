using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class InputPipelineItem
  {
    #region Input

    public KnowledgeBase KnowledgeBase { get; protected set; }

    public Request Request { get; protected set; }

    public ZTokenSequence Input{ get; protected set; }

    public BotState State { get; protected set; }

    #endregion


    #region Output

    public ReactionSet Reactions { get; protected set; }

    #endregion


    public InputPipelineItem(KnowledgeBase kb, BotState state, Request req, ZTokenSequence input)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(state, nameof(state)).IsNotNull();
      Condition.Requires(req, nameof(req)).IsNotNull();

      KnowledgeBase = kb;
      State = state;
      Request = req;
      Input = input;

      Reactions = new ReactionSet();
    }
  }
}
