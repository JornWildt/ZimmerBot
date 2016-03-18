using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    protected ResponseContext Context { get; set; }

    protected CallBinding Inv { get; set; } // FIXME - better name


    public Reaction(ResponseContext rc, CallBinding generator)
    {
      Score = rc.Match.Score;
      Context = rc;
      Inv = generator;
    }


    public string GenerateResponse()
    {
      return Inv.Run(Context);
    }
  }
}
