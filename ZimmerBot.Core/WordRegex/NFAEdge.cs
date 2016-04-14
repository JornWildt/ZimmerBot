using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.WordRegex
{
  public class NFAEdge
  {
    public NFANode Target { get; set; }

    public List<string> EndMatchNames { get; protected set; }

    public NFAEdge(NFANode target)
    {
      Target = target;
      EndMatchNames = new List<string>();
    }
  }
}
