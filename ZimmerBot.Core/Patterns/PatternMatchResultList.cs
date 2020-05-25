using System.Collections.Generic;
using System.Linq;

namespace ZimmerBot.Core.Patterns
{
  public class PatternMatchResultList : List<PatternMatchResult>
  {
    public override string ToString()
    {
      if (Count == 0)
        return "<empty>";
      return this.Select(item => item.ToString()).Aggregate((a, b) => a + " \n " + b);
    }
  }
}
