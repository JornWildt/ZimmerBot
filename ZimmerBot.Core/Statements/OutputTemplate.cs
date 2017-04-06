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
    public string TemplateName { get; set; }

    public List<string> Outputs { get; set; }

    public OutputTemplate(string name, string s, List<string> outputs)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty();
      Condition.Requires(s, nameof(s)).IsNotNull();
      Condition.Requires(outputs, nameof(outputs)).IsNotNull();

      TemplateName = name;
      outputs.Insert(0, s);
      Outputs = outputs;
    }
  }
}
