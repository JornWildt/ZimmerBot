using VDS.RDF;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfNumberValue : RdfValue
  {
    public double Value { get; protected set; }


    public RdfNumberValue(double n)
    {
      Value = n;
    }


    public override INode BuildRdfNode(INodeFactory factory)
    {
      return Value.ToLiteral(factory);
    }
  }
}
