using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Patterns
{
  public class PatternManager
  {
    public List<PatternSet> PatternSets { get; protected set; }


    public PatternManager()
    {
      PatternSets = new List<PatternSet>();
    }


    public void AddPatternSet(PatternSet set)
    {
      Condition.Requires(set, nameof(set)).IsNotNull();

      PatternSets.Add(set);
    }
  }
}
