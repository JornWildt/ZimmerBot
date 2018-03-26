using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using VDS.RDF;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.ConfigParser
{
  public class RdfUriValue : RdfValue
  {
    public string Name { get; protected set; }

    public string Prefix { get; protected set; }

    public RDFStore RDF { get; protected set; }


    public RdfUriValue(List<string> nameElements, RDFStore rdf, string prefix = null)
    {
      Condition.Requires(nameElements, nameof(nameElements)).IsNotEmpty();

      Name = nameElements.Aggregate((a, b) => a + "_" + b);
      Prefix = prefix;
      RDF = rdf;
    }


    public override INode BuildRdfNode(INodeFactory factory)
    {
      string id = StringUtility.Word2Identifier(Name);

      string urlBase = (Prefix != null ? RDF.LookupPrefix(Prefix) : null);

      Uri idUri = (urlBase == null
        ? UrlConstants.ResourceUrl(id)
        : new Uri(urlBase + id));

      return factory.CreateUriNode(idUri);
    }
  }
}
