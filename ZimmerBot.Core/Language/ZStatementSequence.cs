using System.Collections.Generic;


namespace ZimmerBot.Core.Language
{
  public class ZStatementSequence
  {
    public List<ZTokenSequence> Statements { get; protected set; }


    public ZStatementSequence()
    {
      Statements = new List<ZTokenSequence>();
    }
  }
}
