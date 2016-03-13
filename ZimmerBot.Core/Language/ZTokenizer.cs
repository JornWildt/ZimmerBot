using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Language
{
  public class ZTokenizer
  {
    public ZTokenizer()
    {
    }


    public ZTokenString Tokenize(string text)
    {
      IEnumerable<ZToken> tokens = text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => new ZToken(s));
      return new ZTokenString(tokens);
    }
  }
}
