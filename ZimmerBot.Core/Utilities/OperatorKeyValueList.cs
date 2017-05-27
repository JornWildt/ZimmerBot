using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Utilities
{
  public class OperatorKeyValueList : List<OperatorKeyValue>
  {
    public override string ToString()
    {
      return "{ " + this.Select(item => ItemToString(item)).Aggregate((a, b) => a + ", " + b) + "}";
    }


    protected string ItemToString(OperatorKeyValue item)
    {
      return item.Key + " = " + item.Value;
    }
  }
}
