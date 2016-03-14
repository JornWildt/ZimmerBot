using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Parser
{
  public class ZTokenizer
  {
    public ZTokenizer()
    {
    }


    public ZStatementSequence Tokenize(string text)
    {
      ChatParser parser = new ChatParser();
      parser.Parse(text);
      return parser.Result;

      //IEnumerable<ZToken> tokens = text.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => new ZToken(s));
      //return new ZTokenString(tokens);
    }
  }
}
