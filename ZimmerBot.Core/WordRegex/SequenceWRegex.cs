using System.Linq;
using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;
using System;

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


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFAFragment e1 = null, e2 = null;
      NFAFragment start = null;

      foreach (WRegex r in Sequence)
      {
        e2 = r.CalculateNFAFragment(context);
        if (e1 == null)
        {
          start = e2;
        }
        else
        {
          Patch(e1.Out, e2.Start);
        }
        e1 = e2;
      }

      return new NFAFragment(start.Start, e2.Out);
    }

    //public override NFANode CalculateNFA()
    //{
    //  NFANode current = null;
    //  NFANode first = null;

    //  foreach (WRegex r in Sequence)
    //  {
    //    NFANode n = r.CalculateNFA();
    //    if (current == null)
    //    {
    //      current = first = n;
    //    }
    //    else
    //    {
    //      current.Out1 = n;
    //    }
    //  }
    //}
  }
}
