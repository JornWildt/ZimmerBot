using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using log4net;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace ZimmerBot.Core.Knowledge
{
  public class RDFStore
  {
    static ILog Logger = LogManager.GetLogger(typeof(RDFStore));


    protected TripleStore Store { get; set; }

    protected InMemoryDataset Dataset { get; set; }

    protected ISparqlQueryProcessor Processor { get; set; }

    protected SparqlQueryParser SparqlParser { get; set; }

    protected NodeFactory NodeFactory { get; set; }

    protected HashSet<string> LoadedFiles { get; set; }


    public RDFStore()
    {
      Store = new TripleStore();
      Dataset = new InMemoryDataset(Store, true);
      Processor = new LeviathanQueryProcessor(Dataset);
      SparqlParser = new SparqlQueryParser();
      NodeFactory = new NodeFactory();
      LoadedFiles = new HashSet<string>();
    }


    public void LoadFromFile(string filename)
    {
      if (ConfigurationManager.AppSettings["ZimmerBot.RDF.DataDirectory"] != null)
        filename = Path.Combine(ConfigurationManager.AppSettings["ZimmerBot.RDF.DataDirectory"], filename);

      if (!LoadedFiles.Contains(filename))
      {
        Logger.InfoFormat("Loading RDF file '{0}'", filename);
        Store.LoadFromFile(filename);
        LoadedFiles.Add(filename);
      }
      else
        Logger.InfoFormat("Skipping RDF file '{0}' - it has already been loaded", filename);
    }


    public RDFResultSet Query(string s, Dictionary<string, object> matches, IList<object> parameters)
    {
      Condition.Requires(s, nameof(s)).IsNotNull();

      SparqlParameterizedString queryString = new SparqlParameterizedString();
      // queryString.Namespaces.AddNamespace("ex", new Uri("http://example.org/ns#"));
      queryString.CommandText = s;

      if (matches != null)
      {
        foreach (var match in matches)
        {
          queryString.SetParameter(match.Key, NodeFactory.CreateLiteralNode(match.Value.ToString()));
        }
      }

      if (parameters != null)
      {
        for (int i = 0; i < parameters.Count; ++i)
        {
          if (parameters[i] != null)
            queryString.SetParameter("p"+(i+1), NodeFactory.CreateLiteralNode(parameters[i].ToString()));
        }
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
