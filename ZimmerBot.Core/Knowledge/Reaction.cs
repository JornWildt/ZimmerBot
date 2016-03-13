using System;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    protected Func<string> Generator { get; set; }


    public Reaction(double score, Func<string> generator)
    {
      Score = score;
      Generator = generator;
    }


    public string GenerateResponse(ZTokenString input)
    {
      return Generator();
    }
  }
}
