using CuttingEdge.Conditions;
using VDS.RDF;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfZPropertyValue : RdfValue
  {
    public string Name { get; protected set; }

    public RdfZPropertyValue(string name)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty();
      Name = name;
    }


    public override INode BuildRdfNode(INodeFactory factory)
    {
      return factory.CreateUriNode(UrlConstants.PropertyUrl(Name));
    }
  }
}
