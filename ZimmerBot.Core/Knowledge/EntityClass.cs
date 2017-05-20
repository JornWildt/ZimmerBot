using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityClass
  {
    public string ClassName { get; protected set; }

    protected List<WRegexBase> EntityPatterns { get; set; }


    public EntityClass(string className)
    {
      Condition.Requires(className, nameof(className)).IsNotNullOrWhiteSpace();
      ClassName = className;
      EntityPatterns = new List<WRegexBase>();
    }


    public void AddEntity(WRegexBase pattern)
    {
      Condition.Requires(pattern, nameof(pattern)).IsNotNull();
      EntityPatterns.Add(pattern);
    }


    public bool IsEntityMatch(ZTokenSequence input, int i, int j)
    {
      WRegexBase.EvaluationContext context = new WRegexBase.EvaluationContext(input, i, j);

      foreach (WRegexBase expr in EntityPatterns)
      {
        MatchResult result = expr.CalculateNFAMatch(context);
        if (result.Score > 0)
          return true;
      }

      return false;
    }
  }
}
