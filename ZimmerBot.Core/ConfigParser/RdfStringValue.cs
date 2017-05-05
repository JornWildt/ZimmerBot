using CuttingEdge.Conditions;
using VDS.RDF;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfStringValue : RdfValue
  {
    public string Value { get; protected set; }


    public RdfStringValue(string s)
    {
      Condition.Requires(s, nameof(s)).IsNotNull();
      Value = s;
    }


    public override INode BuildRdfNode(INodeFactory factory)
    {
      return Value.ToLiteral(factory);
    }
  }
}
