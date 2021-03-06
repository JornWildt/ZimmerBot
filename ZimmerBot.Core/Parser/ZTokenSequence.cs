﻿using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Parser
{
  public class ZTokenSequence : List<ZToken>
  {
    public ZTokenSequence()
    {
    }


    public ZTokenSequence(IEnumerable<ZToken> input)
      : base(input)
    {
    }


    public override string ToString()
    {
      if (Count == 0)
        return "<empty>";
      return this.Select(t => t.ToString()).Aggregate((a, b) => a + ", " + b);
    }


    public ZToken this[string s]
    {
      get
      {
        return this.FirstOrDefault(t => t.Matches(s));
      }
    }


    public ZTokenSequence CompactEntity(int i, int j, string entityClass, int entityNumber)
    {
      ZTokenSequence result = new ZTokenSequence();
      for (int x = 0; x < i; ++x)
        result.Add(this[x]);
      string s = "";
      for (int x = i; x < j; ++x)
      {
        if (x > i)
          s += " ";
        s += this[x].OriginalText;
      }
      if (entityClass != Constants.IgnoreValue)
        result.Add(new ZTokenEntity(s, entityClass, entityNumber));
      for (int x = j; x < Count; ++x)
        result.Add(this[x]);

      return result;
    }


    public ZTokenSequence CompactConcept(Concept c, int i, int j)
    {
      ZTokenSequence result = new ZTokenSequence();
      for (int x = 0; x < i; ++x)
        result.Add(this[x]);
      string s = "";
      for (int x = i; x < j; ++x)
      {
        if (x > i)
          s += " ";
        s += this[x].OriginalText;
      }
      result.Add(new ZTokenConcept(c, s));
      for (int x = j; x < Count; ++x)
        result.Add(this[x]);

      return result;
    }
  }
}
