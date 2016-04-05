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
using VDS.RDF.Storage;
using VDS.RDF.Writing;

namespace ZimmerBot.Core.Knowledge
{
  public class RDFStore
  {
    static ILog Logger = LogManager.GetLogger(typeof(RDFStore));


    protected bool LoadFilesEnabled { get; set; }

    //protected List<string> InitialFilenames { get; set; }

    protected string ID { get; set; }

    protected TripleStore Store { get; set; }

    protected InMemoryDataset Dataset { get; set; }

    protected ISparqlQueryProcessor Processor { get; set; }

    protected SparqlQueryParser SparqlParser { get; set; }

    protected NodeFactory NodeFactory { get; set; }

    protected Dictionary<string,string> Prefixes { get; set; }

    protected HashSet<string> LoadedFiles { get; set; }


    public RDFStore(string id)
    {
      Condition.Requires(id, nameof(id)).IsNotNull();

      ID = id;

      LoadFilesEnabled = true;
      Store = new TripleStore();
      Dataset = new InMemoryDataset(Store, true);
      Processor = new LeviathanQueryProcessor(Dataset);
      SparqlParser = new SparqlQueryParser();
      NodeFactory = new NodeFactory();
      Prefixes = new Dictionary<string, string>();
      LoadedFiles = new HashSet<string>();

      RDFStoreRepository.Add(this);
    }


    public void Initialize(KnowledgeBase.InitializationMode mode)
    {
      if (mode == KnowledgeBase.InitializationMode.Clear)
        Clear();
      else if (mode == KnowledgeBase.InitializationMode.Restore)
        Restore();
      else if (mode == KnowledgeBase.InitializationMode.RestoreIfExists)
        RestoreIfExists();
    }


    protected void Clear()
    {
      string dbFilename = GetDatabaseFilename();
      File.Delete(dbFilename);
    }


    protected void Restore()
    {
      string dbFilename = GetDatabaseFilename();
      TriGParser trigparser = new TriGParser();
      trigparser.Load(Store, dbFilename);
      LoadFilesEnabled = false;
    }


    protected void RestoreIfExists()
    {
      string dbFilename = GetDatabaseFilename();
      if (File.Exists(dbFilename))
        Restore();
      else
        Clear();
    }


    protected string GetDatabaseFilename()
    {
      string filename = Path.Combine(AppSettings.RDF_DataDirectory, ID + ".trig");
      return filename;
    }


    public void Shutdown()
    {
      string dbFilename = GetDatabaseFilename();
      TriGWriter trigwriter = new TriGWriter();
      trigwriter.Save(Store, dbFilename);
    }


    public void LoadFromFile(string filename)
    {
      if (LoadFilesEnabled)
      {
        if (AppSettings.RDF_ImportDirectory.Value != null)
          filename = Path.Combine(AppSettings.RDF_ImportDirectory.Value, filename);

        if (!LoadedFiles.Contains(filename))
        {
          Logger.InfoFormat("Loading RDF file '{0}'", filename);
          try
          {
            Store.LoadFromFile(filename);
            LoadedFiles.Add(filename);
          }
          catch (Exception ex)
          {
            Logger.Error(ex);
            throw new ApplicationException($"Failed to load RDF file '{filename}': {ex.Message}");
          }
        }
        else
          Logger.InfoFormat("Skipping RDF file '{0}' - it has already been loaded", filename);
      }
    }


    public void DeclarePrefix(string prefix, string url)
    {
      Condition.Requires(prefix, nameof(prefix)).IsNotNull();
      Condition.Requires(url, nameof(url)).IsNotNull();

      Prefixes[prefix] = url;
    }


    public RDFResultSet Query(string s, Dictionary<string, object> matches, IList<object> parameters)
    {
      Condition.Requires(s, nameof(s)).IsNotNull();

      SparqlParameterizedString queryString = new SparqlParameterizedString();
      foreach (var prefix in Prefixes)
        queryString.Namespaces.AddNamespace(prefix.Key, new Uri(prefix.Value));
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
