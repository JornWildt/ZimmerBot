using System.Collections.Generic;

namespace ZimmerBot.Core.Knowledge
{
  public class RDFResultSet : List<Dictionary<string,string>>
  {
    public RDFResultSet()
    {
    }


    public RDFResultSet(IEnumerable<Dictionary<string, string>> source)
      : base(source)
    {
    }
  }
}
