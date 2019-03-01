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


    public EntityManager(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      EntityClasses = new Dictionary<string, EntityClass>();
      KnowledgeBase = kb;
    }


    public void RegisterEntityClass(string className, IList<string> entityNames)
    {
      Condition.Requires(className, nameof(className)).IsNotNullOrWhiteSpace();
      Condition.Requires(entityNames, nameof(entityNames)).IsNotNull();

      EntityClass ec = GetOrCreateClass(className);

      foreach (string entityWord in entityNames)
        ec.AddEntity(WRegex.BuildFromSpaceSeparatedString(entityWord, true));
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

      foreach (string className in classNames)
      {
        EntityClass ec = GetOrCreateClass(className);
        ec.AddEntity(WRegex.BuildFromSpaceSeparatedString(entityName, true));

        foreach (string alt in alternateNames)
        {
          ec.AddEntity(WRegex.BuildFromSpaceSeparatedString(alt,true));
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
    }


    public void FindEntities(ZTokenSequence input, ZTokenSequenceList output)
    {
      FindEntities(input, 0, output, 1);
    }


    protected void FindEntities(ZTokenSequence input, int start, List<ZTokenSequence> output, int entityNumber)
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
              // entityNumber is the index of the found entities.
              // See also "EntityNumber" in "EntityPatternExpr".
              ZTokenSequence result = input.CompactEntity(i, j, ec.Value.ClassName, entityNumber);
              output.Add(result);

              FindEntities(result, i+1, output, entityNumber+1);

              // This is a greedy algortihm, so do not try to match smaller combinations
              i = j;
            }
          }
        }
      }
    }
  }
}
