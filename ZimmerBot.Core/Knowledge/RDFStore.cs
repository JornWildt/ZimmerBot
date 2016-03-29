using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    public RDFResultSet Query(string s)
    {
      SparqlQuery query = SparqlParser.ParseFromString(s);
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
