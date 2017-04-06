using System.Collections.Generic;

namespace ZimmerBot.Core.WordRegex
{
  public class MatchResult
  {
    public double Score { get; set; }

    public Dictionary<string, object> Matches { get; protected set; }


    public MatchResult(double score)
    {
      Score = score;
      Matches = new Dictionary<string, object>();
    }


    public MatchResult(MatchResult src, double score)
    {
      Score = score;
      Matches = new Dictionary<string, object>(src.Matches);
    }


    public MatchResult(double score, Dictionary<string,object> matches)
    {
      Score = score;
      Matches = matches;
    }
  }
}
