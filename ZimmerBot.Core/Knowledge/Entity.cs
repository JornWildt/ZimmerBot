using System;
using System.Linq;
using System.Text.RegularExpressions;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  /// <summary>
  /// This class represent an entity indentified in user input.
  /// </summary>
  public class Entity
  {
    public string Label { get; protected set; }

    public string OriginalLabel { get; protected set; }


    static Regex LabelReducer = new Regex("[^\\w ]");

    public Entity(string label)
    {
      Condition.Requires(label, nameof(label)).IsNotNull();

      OriginalLabel = label;

      string[] words = LabelReducer.Replace(label, " ").Split(' ');
      Label = words.Where(w => !string.IsNullOrEmpty(w)).Aggregate((a, b) => a + " " + b);
    }
  }
}
