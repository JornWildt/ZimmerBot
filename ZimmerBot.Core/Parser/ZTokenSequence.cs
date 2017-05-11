using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Parser
{
  public class ZTokenSequence : List<ZToken>
  {
    public ZTokenSequence()
    {
    }


    public ZTokenSequence(IEnumerable<ZToken> input)
      : base(input)
    {
    }


    public override string ToString()
    {
      return this.Select(t => t.ToString()).Aggregate((a, b) => a + ", " + b);
    }


    public ZToken this[string s]
    {
      get
      {
        return this.FirstOrDefault(t => t.Matches(s));
      }
    }
  }
}
