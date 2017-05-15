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
    public string Word { get; protected set; }

    public List<string> Alternatives { get; protected set; }

    public List<RdfDefinition> RdfDefinitions { get; protected set; }

    public List<string> Classes { get; set; }


    public WordDefinition(string word, List<string> alternatives, List<RdfDefinition> rdf)
    {
      Condition.Requires(word, nameof(word)).IsNotNullOrWhiteSpace();
      Condition.Requires(alternatives, nameof(alternatives)).IsNotNull();
      Condition.Requires(rdf, nameof(rdf)).IsNotNull();

      Word = word;
      Alternatives = alternatives;
      RdfDefinitions = rdf;
    }
  }
}
