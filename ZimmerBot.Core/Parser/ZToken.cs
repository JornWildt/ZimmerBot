using System;
using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Parser
{
  public class ZToken
  {
    public enum TokenType { Word, Number, EMail, Entity }

    public string OriginalText { get; protected set; }

    public TokenType Type { get; protected set; }

    public string EntityClass { get; protected set; }

    public int EntityNumber { get; protected set; }


    public ZToken(string t)
      : this(t, TokenType.Word, null, 0)
    {
    }


    public ZToken(string t, ZToken src)
      : this(t, src.Type, src.EntityClass, src.EntityNumber)
    {
    }


    public ZToken(string t, TokenType type)
      : this(t, type, null, 0)
    {
    }


    public ZToken(string t, string entityClass, int entityNumber = 0)
      : this(t, entityClass == null ? TokenType.Word : TokenType.Entity, entityClass, entityNumber)
    {
    }


    public ZToken(string t, TokenType type, string entityClass, int entityNumber = 0)
    {
      OriginalText = t;
      Type = type;
      EntityClass = entityClass;
      EntityNumber = entityNumber;
    }


    public bool Matches(string v)
    {
      return OriginalText.Equals(v, StringComparison.CurrentCultureIgnoreCase);
    }


    public override string ToString()
    {
      if (Type == TokenType.Entity)
        return $"{OriginalText}[E:{EntityClass}]";
      return OriginalText;
    }
  }
}
