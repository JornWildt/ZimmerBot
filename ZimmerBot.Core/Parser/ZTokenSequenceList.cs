using System.Collections.Generic;
using System.Linq;

namespace ZimmerBot.Core.Parser
{
  public class ZTokenSequenceList : List<ZTokenSequence>
  {
    public override string ToString()
    {
      if (Count == 0)
        return "<empty>";
      return this.Select(t => "(" + t.ToString() + ")").Aggregate((a, b) => a + "/" + b);
    }
  }
}
