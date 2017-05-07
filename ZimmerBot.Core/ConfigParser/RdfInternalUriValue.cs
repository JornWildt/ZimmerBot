using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using VDS.RDF;
using ZimmerBot.Core.Utilities;

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
      string id = StringUtility.Word2Identifier(Name);
      Uri idUri = UrlConstants.FactIdUrl(id);

      return factory.CreateUriNode(idUri);
    }
  }
}
