using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public abstract class WRegex
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


      public MatchResult RegisterMatch(string matchName, object value)
      {
        if (matchName != null && value != null)
          Matches[matchName] = value;
        return this;
      }
    }


    public string MatchName { get; set; }


    public abstract double CalculateSize();

    public abstract WRegex GetLookahead();

    public abstract MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead);
  }
}
