﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace ZimmerBot.Core.Knowledge
{
  public class RDFStore
  {
    protected TripleStore Store { get; set; }

    protected InMemoryDataset Dataset { get; set; }

    protected ISparqlQueryProcessor Processor { get; set; }

    protected SparqlQueryParser SparqlParser { get; set; }


    public RDFStore()
    {
      Store = new TripleStore();
      Dataset = new InMemoryDataset(Store, true);
      Processor = new LeviathanQueryProcessor(Dataset);
      SparqlParser = new SparqlQueryParser();
    }


    public void LoadFromFile(string filename)
    {
      Store.LoadFromFile(filename);
    }


    public RDFResultSet Query(string s, Dictionary<string, object> matches)
    {
      Condition.Requires(s, nameof(s)).IsNotNull();
      Condition.Requires(matches, nameof(matches)).IsNotNull();

      SparqlParameterizedString queryString = new SparqlParameterizedString();
      // queryString.Namespaces.AddNamespace("ex", new Uri("http://example.org/ns#"));
      queryString.CommandText = s;

      foreach (var match in matches)
      {
        queryString.SetParameter(match.Key, new NodeFactory().CreateLiteralNode(match.Value.ToString()));
      }

      SparqlQuery query = SparqlParser.ParseFromString(queryString);

      object result = Processor.ProcessQuery(query);
      return ConvertQueryResult(result);
    }


    private RDFResultSet ConvertQueryResult(object result)
    {
      if (result is SparqlResultSet)
      {
        SparqlResultSet rs = (SparqlResultSet)result;
        var output = new RDFResultSet(rs.Select(r => r.ToDictionary(v => v.Key, v => v.Value.ToString())));
        return output;
      }
      else
        throw new InvalidOperationException($"Could not convert '{result.GetType()}' to RDFResult");
    }
  }
}
