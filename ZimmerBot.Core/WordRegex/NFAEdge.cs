using System.Collections.Generic;

namespace ZimmerBot.Core.WordRegex
{
  public class NFAEdge
  {
    public NFANode Target { get; set; }


    public NFAEdge(NFANode target)
    {
      Target = target;
    }
  }
}
