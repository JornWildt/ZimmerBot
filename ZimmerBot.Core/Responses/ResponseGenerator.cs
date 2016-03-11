using System;
using System.Collections.Generic;


namespace ZimmerBot.Core.Responses
{
  public abstract class ResponseGenerator
  {
    public abstract Func<string> Bind(Dictionary<string, string> input);
  }
}
