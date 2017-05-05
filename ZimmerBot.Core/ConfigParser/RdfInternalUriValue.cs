using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using VDS.RDF;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfInternalUriValue : RdfValue
  {
    public string Name { get; protected set; }


    public RdfInternalUriValue(List<string> nameElements)
    {
      Condition.Requires(nameElements, nameof(nameElements)).IsNotEmpty();

      Name = nameElements.Aggregate((a, b) => a + "_" + b);
    }


    public override INode BuildRdfNode(INodeFactory factory)
    {
      // FIXME: configurable
      Uri nameUri = new Uri("http://zimmerbot/stuff#" + Name);

      return factory.CreateUriNode(nameUri);
    }
  }
}
