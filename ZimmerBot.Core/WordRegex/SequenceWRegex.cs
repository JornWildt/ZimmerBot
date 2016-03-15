using System.Linq;
using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class SequenceWRegex : WRegex
  {
    public List<WRegex> Sequence { get; protected set; }


    public SequenceWRegex()
    {
      Sequence = new List<WRegex>();
    }


    public void Add(WRegex p)
    {
      Sequence.Add(p);
    }


    public override double CalculateSize()
    {
      return Sequence.Sum(w => w.CalculateSize());
    }


    public override WRegex GetLookahead()
    {
      return this;
    }


    public override double CalculateTriggerScore(EvaluationContext context, WRegex lookahead)
    {
      double score = 1;

      for (int i = 0; i < Sequence.Count; ++i)
      {
        WRegex lookahead2 = (i < Sequence.Count - 1 ? Sequence[i + 1].GetLookahead() : lookahead);
        double s = Sequence[i].CalculateTriggerScore(context, lookahead2);
        score = score * s;
      }

      return score;
    }
  }
}
