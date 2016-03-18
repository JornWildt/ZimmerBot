using System;
using ZimmerBot.Core.Parser;
using static ZimmerBot.Core.Knowledge.ProcessorRegistry;

namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    protected ResponseContext Context { get; set; }

    protected Invocation Inv { get; set; } // FIXME - better name


    public Reaction(ResponseContext rc, Invocation generator)
    {
      Score = rc.Match.Score;
      Context = rc;
      Inv = generator;
    }


    public string GenerateResponse()
    {
      //ProcessorInput pi = new ProcessorInput(Context, Inv);
      //if (Inv == null)
      //  return "DUH!";

      return Inv.Run(Context);
    }
  }
}
