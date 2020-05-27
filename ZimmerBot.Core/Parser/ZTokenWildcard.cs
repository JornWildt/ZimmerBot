using System;
using ZimmerBot.Core.Patterns;

namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public class ZTokenWildcard : ZToken
  {
    public int WildcardNumber { get; protected set; }

    private string _toString;

    private int _size;

    
    public ZTokenWildcard(string t, int wildcardNumber, int size)
      : base(t)
    {
      WildcardNumber = wildcardNumber;
      _toString = $"<{OriginalText}>";
      _size = size;
    }


    public override string ToString() => _toString;


    public override ZToken CorrectWord(string word) => new ZTokenWildcard(word, WildcardNumber, Size);

    public override string GetKey() => WildcardPatternExpr.GetIdentifier(WildcardNumber);

    public override string GetUntypedKey() => WildcardPatternExpr.GetIdentifier(WildcardNumber);

    public override int Size => _size;
  }
}
