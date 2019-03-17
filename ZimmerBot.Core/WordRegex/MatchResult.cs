using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.WordRegex
{
  // Serializable since we store previoius matches in the session
  [Serializable]
  public class MatchResult
  {
    public double Score { get; set; }

    public Dictionary<string, ZToken> Matches { get; protected set; }


    private Dictionary<string, object> _matchedTexts;
    public Dictionary<string, object> MatchedTexts
    {
      get
      {
        if (_matchedTexts == null)
          _matchedTexts = Matches.ToDictionary(item => item.Key, item => (object)item.Value.OriginalText);
        return _matchedTexts;
      }
    }


    public MatchResult(double score)
    {
      Score = score;
      Matches = new Dictionary<string, ZToken>();
    }


    public MatchResult(MatchResult src, double score)
    {
      Score = score;
      Matches = new Dictionary<string, ZToken>(src.Matches);
    }


    public MatchResult(double score, Dictionary<string, ZToken> matches)
    {
      Score = score;
      Matches = matches;
    }
  }
}
