using System;

namespace ZimmerBot.Core.Knowledge
{
  public class RepetitionException : Exception
  {
    public RepetitionException(string msg)
      : base(msg)
    {
    }
  }
}
