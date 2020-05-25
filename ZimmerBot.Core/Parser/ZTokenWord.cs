using System;

namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public class ZTokenWord : ZToken
  {
    private string _toString;


    public ZTokenWord(string word)
      : base(word)
    {
      _toString = $"\"{OriginalText}\"";
    }

    public override string ToString() => _toString;


    public override ZToken CorrectWord(string word) => new ZTokenWord(word);

    public override string GetKey() => OriginalText;

    public override string GetUntypedKey() => OriginalText;
  }
}
