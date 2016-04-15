using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.WordRegex
{
  public class WordWRegex : WRegex
  {
    public string Word { get; set; }


    public WordWRegex(string w)
    {
      Condition.Requires(w, "w").IsNotNull();
      Word = w;
    }


    public override double CalculateSize()
    {
      return 1;
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFANode node = NFANode.CreateLiteral(context, Word);
      return new NFAFragment(node, node.Out);
    }
  }
}
