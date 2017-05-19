using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityManager
  {
    public Dictionary<string, EntityClass> EntityClasses { get; protected set; }

    protected KnowledgeBase KnowledgeBase { get; set; }

    static Regex LabelReducer = new Regex("[^\\w ]");


    public EntityManager(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      EntityClasses = new Dictionary<string, EntityClass>();
      KnowledgeBase = kb;
    }


    public void RegisterEntityClass(string className, IEnumerable<string> entityNames)
    {
      List<List<string>> names = new List<List<string>>();
      names.Add(entityNames.ToList());
      // FIXME RegisterEntityClass(className, names);
      //for (int pos = 0; pos < entityNames.Count; ++pos)
      //{
      //  Condition.Requires(entityNames[pos], nameof(entityNames) + "[" + pos + "]").IsNotNull();

      //  // List of list of entity names implies each name is exactly one word
      //  foreach (string entityWord in entityNames[pos])
      //    ec.AddEntityWord(entityWord, pos);
      //}

    }


    public void RegisterEntityClass(string className, List<WRegexBase> entityPatterns)
    {
      Condition.Requires(className, nameof(className)).IsNotNullOrWhiteSpace();
      Condition.Requires(entityPatterns, nameof(entityPatterns)).IsNotNull();

      EntityClass ec = GetOrCreateClass(className);

      foreach (WRegexBase entityPatern in entityPatterns)
        ec.AddEntity(entityPatern);
    }


    public void RegisterEntity(string entityName, List<string> alternateNames, List<string> classNames)
    {
      Condition.Requires(entityName, nameof(entityName)).IsNotNullOrWhiteSpace();
      Condition.Requires(classNames, nameof(classNames)).IsNotNull();
      Condition.Requires(alternateNames, nameof(alternateNames)).IsNotNull();

      entityName = LabelReducer.Replace(entityName, "");

      foreach (string className in classNames)
      {
        EntityClass ec = GetOrCreateClass(className);
        ec.AddEntity(new LiteralWRegex(entityName));

        foreach (string alt in alternateNames)
        {
          string alt2 = LabelReducer.Replace(alt, "");
          ec.AddEntity(new LiteralWRegex(alt2));
        }
      }
    }


    protected EntityClass GetOrCreateClass(string className)
    {
      if (!EntityClasses.ContainsKey(className))
        EntityClasses[className] = new EntityClass(className);

      return EntityClasses[className];
    }


    public void SetupComplete()
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


    public void FindEntities(ZTokenSequence input, ZTokenSequenceList output)
    {
      FindEntities(input, 0, output);
    }


    protected void FindEntities(ZTokenSequence input, int start, List<ZTokenSequence> output)
    {
      foreach (var ec in EntityClasses)
      {
        for (int i = start; i < input.Count; ++i)
        {
          for (int j = input.Count; j > i; --j)
          {
            bool isEntity = ec.Value.IsEntityMatch(input, i, j);
            if (isEntity)
            {
              ZTokenSequence result = input.CompactEntity(i, j, ec.Value.ClassName);
              output.Add(result);

              if (j != i+1)
                FindEntities(result, j-1, output);

              // This is a greedy algortihm, so do not try to match smaller combinations
              i = j;
            }
          }
        }
      }
    }
  }
}
