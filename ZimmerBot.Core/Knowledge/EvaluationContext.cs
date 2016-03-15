using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class EvaluationContext
  {
    public BotState State { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public int CurrentTokenIndex { get; set; }


    public EvaluationContext(BotState state, ZTokenSequence input)
    {
      Condition.Requires(state, "state").IsNotNull();
      Condition.Requires(input, "input").IsNotNull();

      State = state;
      Input = input;
    }
  }
}
