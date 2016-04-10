using System.Linq;
using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class SequenceWRegex : WRegex
  {
    public List<WRegex> Sequence { get; protected set; }


    public SequenceWRegex()
      : this(null)
    {
    }


    public SequenceWRegex(string matchName)
    {
      MatchName = matchName;
      Sequence = new List<WRegex>();
    }


    public SequenceWRegex Add(WRegex p)
    {
      Sequence.Add(p);
      return this;
    }


    public void Insert(int index, WRegex p)
    {
      Sequence.Insert(0, p);
    }


    public override double CalculateSize()
    {
      return Sequence.Sum(w => w.CalculateSize());
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override MatchResult CalculateMatchResult(TriggerEvaluationContext context, WRegex lookahead)
    {
      MatchResult result = new MatchResult(1, "");

      for (int i = 0; i < Sequence.Count; ++i)
      {
        WRegex lookahead2 = (i < Sequence.Count - 1 ? Sequence[i + 1].GetLookahead() : lookahead);
        MatchResult subResult = Sequence[i].CalculateMatchResult(context, lookahead2);
        result = MatchResult.Sequence(result, subResult);
      }

      // FIXME: transfer match values

      return result;
    }
  }
}
