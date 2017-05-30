using System.Collections.Generic;
using System.Linq;

namespace ZimmerBot.Core.Utilities
{
  public class OperatorKeyValueList : List<OperatorKeyValue>
  {
    public override string ToString()
    {
      return "{ " + this.Select(item => item.ToString()).Aggregate((a, b) => a + ", " + b) + "}";
    }
  }
}
