namespace ZimmerBot.Core.Parser
{
  public class ZTokenizer
  {
    public ZTokenizer()
    {
    }


    public ZStatementSequence Tokenize(string text)
    {
      if (text == null)
        return null;

      ChatParser parser = new ChatParser();
      parser.Parse(text);
      return parser.Result;
    }
  }
}
