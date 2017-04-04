using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Statements
{
  public class OutputTemplate
  {
    public string Key { get; set; }

    public List<string> Outputs { get; set; }

    public string Value
    {
      get { return Outputs.Aggregate((a, b) => a + "\n" + b); }
    }

    public OutputTemplate(string key, string s, List<string> outputs)
    {
      Condition.Requires(key, nameof(key)).IsNotNullOrEmpty();
      Condition.Requires(s, nameof(s)).IsNotNull();
      Condition.Requires(outputs, nameof(outputs)).IsNotNull();

      Key = key;
      outputs.Insert(0, s);
      Outputs = outputs;
    }
  }
}
