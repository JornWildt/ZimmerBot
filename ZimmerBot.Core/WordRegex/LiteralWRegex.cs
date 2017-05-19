using System;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.WordRegex
{
  public class LiteralWRegex : WRegexBase
  {
    public string Literal { get; set; }


    public LiteralWRegex(string l)
    {
      Condition.Requires(l, nameof(l)).IsNotNull();
      Literal = l;
    }


    public override void ExtractWordsForSpellChecker()
    {
      SpellChecker.AddWord(Literal);
    }


    public override double CalculateSize()
    {
      return 1;
    }


    public override NFAFragment CalculateNFAFragment(EvaluationContext context)
    {
      NFANode node = NFANode.CreateLiteral(context, Literal);
      return new NFAFragment(node, node.Out);
    }


    public override string ToString()
    {
      return Literal;
    }
  }
}
