using System.Collections.Generic;

namespace ZimmerBot.Core.WordRegex
{
  public class NFAFragment
  {
    public NFANode Start { get; set; }

    public List<NFAEdge> Out { get; set; }

    public NFAFragment(NFANode start, List<NFAEdge> os)
    {
      Start = start;
      Out = new List<NFAEdge>(os);
    }
  }
}
