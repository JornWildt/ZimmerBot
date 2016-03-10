using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Language
{
  public class Tokenizer
  {
    public Tokenizer()
    {
    }


    public TokenString Tokenize(string text)
    {
      IEnumerable<Token> tokens = text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => new Token(s));
      return new TokenString(tokens);
    }
  }
}
