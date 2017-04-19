using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class EntityPatternExpr : PatternExpr
  {
    public string ParameterName { get; protected set; }

    public string EntityClass { get; protected set; }


    public EntityPatternExpr(string parameterName, string entityClass)
    {
      Condition.Requires(parameterName, nameof(parameterName)).IsNotNullOrWhiteSpace();
      Condition.Requires(entityClass, nameof(entityClass)).IsNotNullOrWhiteSpace();

      ParameterName = parameterName;
      EntityClass = entityClass;
    }


    public override string Identifier
    {
      get { return GetIdentifier(EntityClass); }
    }


    public static string GetIdentifier(string name)
    {
      return "Entity:" + name;
    }


    public override void ExtractMatchValues(Dictionary<string, string> matchValues, Queue<ZToken> entityTokens)
    {
      if (entityTokens.Count > 0)
      {
        ZToken entity = entityTokens.Dequeue();
        matchValues[ParameterName] = entity.OriginalText;
      }
    }
  }
}
