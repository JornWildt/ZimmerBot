using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Utilities
{
  public class OperatorKeyValue
  {
    public string Key { get; protected set; }

    public string Operator { get; protected set; }

    public string Value { get; protected set; }


    public OperatorKeyValue(string key, string op, string value)
    {
      Condition.Requires(key, nameof(key)).IsNotNullOrWhiteSpace();
      Condition.Requires(op, nameof(op)).IsNotNullOrWhiteSpace();

      Key = key;
      Operator = op;
      Value = value;
    }


    public override string ToString()
    {
      return Key + " " + Operator + " " + Value;
    }
  }
}
