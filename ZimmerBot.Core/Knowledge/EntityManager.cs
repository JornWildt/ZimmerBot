using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityManager
  {
    public Dictionary<string, EntityClass> EntityClasses { get; protected set; }

    public EntityManager()
    {
      EntityClasses = new Dictionary<string, EntityClass>();
    }


    public void RegisterEntityClass(string className, IEnumerable<string> entityNames)
    {
      if (!EntityClasses.ContainsKey(className))
        EntityClasses[className] = new EntityClass(className);

      EntityClass ec = EntityClasses[className];

      foreach (string entityName in entityNames)
        ec.AddEntity(entityName);
    }


    public void UpdateStatistics()
    {
      double totalNumberOfWords = EntityClasses.Select(c => c.Value.TotalNumberOfWords).Sum();
      foreach (var entry in EntityClasses)
        entry.Value.UpdateStatistics(totalNumberOfWords);
    }


    private class Label
    {
      public string Name { get; protected set; }
      public int Position { get; protected set; }
      public Label(string name, int pos)
      {
        Name = name;
        Position = pos;
      }
    }


    public ZTokenSequence CalculateLabels(ZTokenSequence tokens)
    {
      Label[] labels = new Label[tokens.Count];
      for (int t = 0; t < tokens.Count; ++t)
      {
        string word = tokens[t].OriginalText;
        labels[t] = new Label(null, 0);

        foreach (EntityClass ec in EntityClasses.Values)
        {
          for (int pos = 0; pos < ec.LongestWordCount; ++pos)
          {
            double p = ec.ProbabilityFor(word, pos);
            if (p > 0.0 && (pos == 0 || t > 0 && labels[t - 1].Name == ec.ClassName && labels[t - 1].Position == pos - 1))
            {
              labels[t] = new Label(ec.ClassName, pos);
            }
          }
        }
      }

      ZTokenSequence result = new ZTokenSequence();
      for (int i = 0; i < tokens.Count; ++i)
      {
        string name = tokens[i].OriginalText;

        if (i < tokens.Count - 1)
        {
          for (int j = i + 1; j < tokens.Count && (labels[j].Name == labels[j - 1].Name && labels[j].Position == labels[j - 1].Position + 1); ++j, ++i)
            name += " " + tokens[j].OriginalText;
        }

        result.Add(new ZToken(name, labels[i].Name));
      }

      return result;
    }
  }
}
