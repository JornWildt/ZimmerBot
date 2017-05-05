using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfDefinition
  {
    public string Name { get; protected set; }

    public List<RdfValue> Values { get; protected set; }


    public RdfDefinition(string name, List<RdfValue> values)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
      Condition.Requires(values, nameof(values)).IsNotEmpty();

      Name = name;
      Values = values;
    }
  }
}
