using System.Collections.Generic;


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
  }
}
