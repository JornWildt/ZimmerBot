using System;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Patterns;

namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public class ZTokenConcept : ZToken
  {
    public Concept Concept { get; protected set; }

    private string _toString;

    public ZTokenConcept(Concept c, string t)
      : base(t)
    {
      Concept = c;
      _toString = $"%{c.Name}";
    }


    public override string ToString() => _toString;


    public override ZToken CorrectWord(string word) => throw new NotImplementedException("Cannot correct words in concept token.");

    public override string GetKey() => _toString;

    public override string GetUntypedKey() => _toString;

  }
}
