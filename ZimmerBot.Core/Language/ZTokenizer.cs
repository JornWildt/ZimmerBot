using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Language
{
  public class ZTokenizer
  {
    public ZTokenizer()
    {
    }


    public ZTokenString Tokenize(string text)
    {
      ChatParser parser = new ChatParser();
      parser.Parse(text);
      return parser.Result;

      //IEnumerable<ZToken> tokens = text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => new ZToken(s));
      //return new ZTokenString(tokens);
    }
  }
}
