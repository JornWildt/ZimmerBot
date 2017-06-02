using System;
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

      ParameterName = parameterName;
      EntityClass = entityClass;
      _weight = (EntityClass == Constants.StarValue ? 0.5 : 0.75);
    }


    public override string Identifier
    {
      get { return GetIdentifier(EntityClass); }
    }


    protected double _weight;
    public override double Weight
    {
      get { return _weight; }
    }


    public override double ProbabilityFactor
    {
      get { return 1.0; }
    }


    public static string GetIdentifier(string name)
    {
      return "Entity:" + name;
    }


    public override string ToString()
    {
      return "{" + EntityClass + ":" + ParameterName + "}";
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens)
    {
      if (entityTokens.Count > 0)
      {
        ZToken entity = entityTokens.Dequeue();
        matchValues[ParameterName] = entity;
      }
    }
  }
}
