using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  /// <summary>
  /// Represents zero or more repetisions of a predicate
  /// </summary>
  public class RepetitionWRegex : WRegexBase
  {
    public WRegexBase Sub { get; protected set; }

    public int MinCount { get; protected set; }

    public int MaxCount { get; protected set; }


    public RepetitionWRegex(WRegexBase a)
      : this(a, 0, 9999)
    {
    }


    public RepetitionWRegex(WRegexBase a, int min, int max)
    {
      Condition.Requires(a, "a").IsNotNull();
      Sub = a;
      MinCount = min;
      MaxCount = max;
    }


    public override double CalculateSize()
    {
      return Sub.CalculateSize();
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFAFragment e = Sub.CalculateNFAFragment(context);
      NFANode s = NFANode.CreateSplit(context, e.Start, null);

      if (MinCount == 0 && MaxCount == 1)
      {
        e.Out.Add(s.Out[1]);
        return new NFAFragment(s, e.Out);
      }
      else if (MinCount == 0 && MaxCount == 9999)
      {
        PatchNFAEdges(e.Out, s);
        return new NFAFragment(s, s.Out);
      }
      else if (MinCount == 1 && MaxCount == 9999)
      {
        PatchNFAEdges(e.Out, s);
        return new NFAFragment(e.Start, s.Out);
      }
      else
        throw new NotImplementedException();
    }


    public override string ToString()
    {
      return Sub.ToString() + "{" + MinCount + "," + MaxCount + "}";
    }
  }
}
