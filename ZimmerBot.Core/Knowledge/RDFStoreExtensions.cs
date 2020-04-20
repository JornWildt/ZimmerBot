using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace ZimmerBot.Core.Knowledge
{
  public static class RDFStoreExtensions
  {
    public static object GetSingleAttributeOfEntityKnownBy(this RDFStore store, string name, Uri attr)
    {
      string query = @"
select ?attr
where
{
  ?subj @pred ?attr.
  ?subj zp:knownby ?knownby.
  FILTER (?knownby = lcase(@name))
}";

      RDFResultSet output = store.Query(
        query,
        new Dictionary<string, object> 
        { 
          ["name"] = name,
          ["pred"] = store.NodeFactory.CreateUriNode(attr)
        },
        new List<object>());

      if (output.Count > 0 && output[0].TryGetValue("attr", out string result))
        return result;
      else
        return null;
    }
  }
}
