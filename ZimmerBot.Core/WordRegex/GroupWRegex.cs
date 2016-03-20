using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class GroupWRegex : WRegex
  {
    public WRegex Sub { get; protected set; }


    public GroupWRegex(WRegex sub)
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
      result.RegisterMatch((context.CurrentRepetitionIndex++).ToString(), result.MatchedText);
      return result;
    }
  }
}
