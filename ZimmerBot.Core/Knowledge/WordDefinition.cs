using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinition
  {
    public string Word { get; protected set; }

    public List<string> Alternatives { get; protected set; }


    public WordDefinition(string word, List<string> alternatives)
    {
      Condition.Requires(word, nameof(word)).IsNotNullOrWhiteSpace();
      Condition.Requires(alternatives, nameof(alternatives)).IsNotNull();

      Word = word;
      Alternatives = alternatives;
    }
  }
}
