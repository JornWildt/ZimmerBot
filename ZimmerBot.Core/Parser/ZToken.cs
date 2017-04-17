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


    public ZToken(string t)
      : this(t, TokenType.Word)
    {
    }


    public ZToken(string t, ZToken src)
      : this(t, src.Type)
    {
    }


    public ZToken(string t, TokenType type)
    {
      OriginalText = t;
      Type = type;
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
