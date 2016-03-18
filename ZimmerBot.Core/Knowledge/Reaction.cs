using CuttingEdge.Conditions;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    protected ResponseContext Context { get; set; }

    protected CallBinding ResponseBinding { get; set; } // FIXME - better name


    public Reaction(ResponseContext context, CallBinding b)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();
      Condition.Requires(b, nameof(b)).IsNotNull();

      Score = context.Match.Score;
      Context = context;
      ResponseBinding = b;
    }


    public string GenerateResponse()
    {
      return ResponseBinding.Invoke(Context);
    }
  }
}
