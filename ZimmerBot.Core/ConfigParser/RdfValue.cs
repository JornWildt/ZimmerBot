using VDS.RDF;

namespace ZimmerBot.Core.ConfigParser
{
  public abstract class RdfValue
  {
    public abstract INode BuildRdfNode(INodeFactory factory);
  }
}
