using System;
using System.Collections.Generic;
using System.Linq;


namespace ZimmerBot.Core.Parser
{
  [Serializable]
  public abstract class ZToken
  {
    public string OriginalText { get; protected set; }



    public ZToken(string t)
    {
      OriginalText = t;
    }


    public abstract ZToken CorrectWord(string word);

    public abstract string GetKey();

    public abstract string GetUntypedKey();


    public bool Matches(string v)
    {
      return OriginalText.Equals(v, StringComparison.CurrentCultureIgnoreCase);
    }
  }
}
