using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class NegationWRegex : WRegex
  {
    public WRegex Sub { get; protected set; }


    public NegationWRegex(WRegex sub)
    {
      Sub = sub;
    }


    public override double CalculateSize()
    {
      return Sub.CalculateSize();
    }


    public override WRegex GetLookahead()
    {
      return Sub.GetLookahead();
    }


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      MatchResult result = Sub.CalculateMatchResult(context, lookahead);
      return new MatchResult(1 - result.Score, result.MatchedText).RegisterMatch(MatchName, result.MatchedText);
    }
  }
}
