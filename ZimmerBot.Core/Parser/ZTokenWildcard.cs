using System;
using ZimmerBot.Core.Patterns;

namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public class ZTokenWildcard : ZToken
  {
    public int WildcardNumber { get; protected set; }

    private string _toString;

    
    public ZTokenWildcard(string t, int wildcardNumber = 0)
      : base(t)
    {
      WildcardNumber = wildcardNumber;
      _toString = $"<{OriginalText}>";
    }


    public override string ToString() => _toString;


    public override ZToken CorrectWord(string word) => new ZTokenWildcard(word, WildcardNumber);

    public override string GetKey() => WildcardPatternExpr.GetIdentifier(WildcardNumber);

    public override string GetUntypedKey() => WildcardPatternExpr.GetIdentifier(WildcardNumber);
  }
}
