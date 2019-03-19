using System.Collections.Generic;
using System.Linq;

namespace ZimmerBot.Core.Utilities
{
  public class OperatorKeyValueList : List<OperatorKeyValue>
  {
    private string _toString;

    public override string ToString()
    {
      if (_toString == null)
        _toString = "{ " + this.Select(item => item.ToString()).Aggregate((a, b) => a + ", " + b) + "}";
      return _toString;
    }
  }
}
