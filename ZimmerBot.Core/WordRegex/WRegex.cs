using System;
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


      public MatchResult(MatchResult src, double score)
      {
        Score = score;
        Matches = new Dictionary<string, object>(src.Matches);
      }


      public MatchResult RegisterMatch(string matchName, object value)
      {
        if (matchName != null && value != null)
          Matches[matchName] = value;
        return this;
      }

      public static MatchResult Sequence(MatchResult a, MatchResult b)
      {
        if (a == null && b == null)
          return null;
        else if (a == null)
          return b;
        else if (b == null)
          return a;

        MatchResult v = new MatchResult(a.Score * b.Score);

        foreach (var item in a.Matches)
          v.Matches[item.Key] = item.Value;
        foreach (var item in b.Matches)
          v.Matches[item.Key] = item.Value;

        return v;
      }
    }


    public string MatchName { get; set; }


    public abstract double CalculateSize();

    public abstract WRegex GetLookahead();

    public abstract MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead);
  }
}
