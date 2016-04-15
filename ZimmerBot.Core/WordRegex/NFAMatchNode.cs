using System.Collections.Generic;

namespace ZimmerBot.Core.WordRegex
{
  public class NFAMatchNode
  {
    public NFANode Node { get; set; }

    public Dictionary<string, string> Matches { get; set; }

    public NFAMatchNode(NFANode n)
    {
      Node = n;
      Matches = new Dictionary<string, string>();
    }

    public NFAMatchNode(NFANode n, Dictionary<string, string> matches)
    {
      Node = n;
      Matches = new Dictionary<string, string>(matches);
    }
  }
}
