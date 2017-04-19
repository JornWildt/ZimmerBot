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


    public ZToken(string t)
      : this(t, TokenType.Word, null)
    {
    }


    public ZToken(string t, ZToken src)
      : this(t, src.Type, src.EntityClass)
    {
    }


    public ZToken(string t, TokenType type)
      : this(t, type, null)
    {
    }


    public ZToken(string t, string entityClass)
      : this(t, entityClass == null ? TokenType.Word : TokenType.Entity, entityClass)
    {
    }


    public ZToken(string t, TokenType type, string entityClass)
    {
      OriginalText = t;
      Type = type;
      EntityClass = entityClass;
    }


    public bool Matches(string v)
    {
      return OriginalText.Equals(v, StringComparison.CurrentCultureIgnoreCase);
    }


    public override string ToString()
    {
      return OriginalText;
    }
  }
}
