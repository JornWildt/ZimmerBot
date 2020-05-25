using System;
using ZimmerBot.Core.Patterns;

namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public class ZTokenEntity : ZToken
  {
    public string EntityClass { get; protected set; }

    public int EntityNumber { get; protected set; }

    private string _toString;


    public ZTokenEntity(string t, string entityClass, int entityNumber = 0)
      : base(t)
    {
      EntityClass = entityClass;
      EntityNumber = entityNumber;
      _toString = $"{OriginalText}[E:{EntityClass}]";
    }


    public override string ToString() => _toString;


    public override ZToken CorrectWord(string word) => new ZTokenEntity(word, EntityClass, EntityNumber);

    public override string GetKey() => EntityPatternExpr.GetIdentifier(EntityClass, EntityNumber);

    public override string GetUntypedKey() => EntityPatternExpr.GetIdentifier("", EntityNumber);
  }
}
