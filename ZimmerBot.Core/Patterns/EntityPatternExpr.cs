using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class EntityPatternExpr : PatternExpr
  {
    public string ParameterName { get; protected set; }

    private string _entityClass;
    public string EntityClass
    {
      get
      {
        return _entityClass;
      }
      set
      {
        _entityClass = value;
        _identifier = GetIdentifier(value, EntityNumber);
        _toString = "{" + EntityClass + ":" + ParameterName + "}";
      }
    }

    private int _entityNumber;

    // EntityNumber is the index of the Entity in the expression list it is
    // included in. This avoids two entities (number one and two) in a token input
    // matching a single input in a pattern with only one entity.
    public int EntityNumber
    {
      get
      {
        return _entityNumber;
      }
      set
      {
        _entityNumber = value;
        _identifier = GetIdentifier(EntityClass, value);
        _toString = "{" + EntityClass + ":" + ParameterName + "}";
      }
    }

    private string _toString;


    public EntityPatternExpr(string parameterName, string entityClass)
    {
      Condition.Requires(parameterName, nameof(parameterName)).IsNotNullOrWhiteSpace();

      ParameterName = parameterName;
      EntityClass = entityClass;
      _weight = (EntityClass == Constants.StarValue ? 0.5 : 0.75);
    }


    public override void UpdateEntityNumber(ref int entityNumber)
    {
      EntityNumber = entityNumber++;
    }


    private string _identifier;

    public override string Identifier => _identifier;


    protected double _weight;
    public override double Weight
    {
      get { return _weight; }
    }


    public override double ProbabilityFactor
    {
      get { return 1.0; }
    }


    public static string GetIdentifier(string name, int index)
    {
      return "Entity-" + index + ":" + name;
    }


    public override string ToString() => _toString;


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


    public override void RegisterReferencedParameter(HashSet<string> p)
    {
      p.Add(ParameterName);
    }


    public override double CalculateMatch(ZTokenSequence input, int myPos, List<PatternExpr> expressions)
    {
      // FIXME: Handle "star" entity class

      for (int i = 0; i < input.Count; ++i)
      {
        if (input[i] is ZTokenEntity te
            && te.EntityNumber == EntityNumber
            && (EntityClass == null || te.EntityClass == EntityClass))
        {
          int dist = i - myPos;
          return (double)(expressions.Count - Math.Abs(dist)) / (double)expressions.Count;
        }
      }

      return -0.5;
    }
  }
}
