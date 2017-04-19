using CuttingEdge.Conditions;

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
  }
}
