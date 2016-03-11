using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Language
{
  public class TokenString : List<Token>
  {
    public TokenString()
    {
    }


    public TokenString(IEnumerable<Token> input)
      : base(input)
    {
    }


    public override string ToString()
    {
      return this.Select(t => t.ToString()).Aggregate((a, b) => a + "," + b);
    }


    public Token this[string s]
    {
      get
      {
        return this.FirstOrDefault(t => t.Matches(s));
      }
    }
  }
}
