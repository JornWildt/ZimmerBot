using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;


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


    public override double CalculateSize()
    {
      return 1;
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
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
