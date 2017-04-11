using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Statements
{
  public class OutputTemplate
  {
    public string Id { get; protected set; }

    public string TemplateName { get; protected set; }

    public List<string> Outputs { get; protected set; }


    public OutputTemplate(string name, string s)
      : this(name, s, new List<string>())
    {
    }


    public OutputTemplate(string name, string s, List<string> outputs)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty();
      Condition.Requires(s, nameof(s)).IsNotNull();
      Condition.Requires(outputs, nameof(outputs)).IsNotNull();

      TemplateName = name;
      outputs.Insert(0, s);
      Outputs = outputs;

      Id = CryptoHelper.CalculateChecksum(outputs.Aggregate((a, b) => a + "|" + b));
    }
  }
}
