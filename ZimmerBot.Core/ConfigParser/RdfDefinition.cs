using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfDefinition
  {
    public RdfValue Key { get; protected set; }

    public List<RdfValue> Values { get; protected set; }


    public RdfDefinition(RdfValue key, List<RdfValue> values)
    {
      Condition.Requires(key, nameof(key)).IsNotNull();
      Condition.Requires(values, nameof(values)).IsNotEmpty();

      Key = key;
      Values = values;
    }
  }
}
