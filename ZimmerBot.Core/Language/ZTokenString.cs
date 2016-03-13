using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Language
{
  public class ZTokenString : List<ZToken>
  {
    public ZTokenString()
    {
    }


    public ZTokenString(IEnumerable<ZToken> input)
      : base(input)
    {
    }


    public override string ToString()
    {
      return this.Select(t => t.ToString()).Aggregate((a, b) => a + "," + b);
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
