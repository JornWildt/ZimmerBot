using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.ConfigParser;

namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinition
  {
    // Null word means "no word" = anonymous definition
    public string Word { get; protected set; }

    /* Optional ID */
    public string Id { get; protected set; }

    public List<string> Alternatives { get; protected set; }

    public List<RdfDefinition> RdfDefinitions { get; protected set; }

    public List<string> Classes { get; set; }


    public WordDefinition(string word, string id, List<string> alternatives, List<RdfDefinition> rdf)
    {
      Condition.Requires(alternatives, nameof(alternatives)).IsNotNull();
      Condition.Requires(rdf, nameof(rdf)).IsNotNull();

      Word = word;
      Id = id ?? word;
      Alternatives = alternatives;
      RdfDefinitions = rdf;
    }
  }
}
