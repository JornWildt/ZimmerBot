using CuttingEdge.Conditions;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class EvaluationContext
  {
    public BotState State { get; protected set; }

    public ZTokenString Input { get; protected set; }



    public EvaluationContext(BotState state, ZTokenString input)
    {
      Condition.Requires(state, "state").IsNotNull();
      Condition.Requires(input, "input").IsNotNull();

      State = state;
      Input = input;
    }
  }
}
