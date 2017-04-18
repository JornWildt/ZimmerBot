using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityManager
  {
    public Dictionary<string, EntityClass> EntityClasses { get; protected set; }

    public EntityManager()
    {
      EntityClasses = new Dictionary<string, EntityClass>();
    }


    public void RegisterEntityClass(string className, List<string> entityNames)
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
  }
}
