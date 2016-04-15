using System.Linq;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using System;

namespace ZimmerBot.Core.WordRegex
{
  public class ChoiceWRegex : WRegexBase
  {
    public List<WRegexBase> Choices { get; protected set; }


    public ChoiceWRegex()
      : this(Enumerable.Empty<WRegexBase>())
    {
    }


    public ChoiceWRegex(WRegexBase left, WRegexBase right)
    {
      Condition.Requires(left, "left").IsNotNull();
      Condition.Requires(right, "right").IsNotNull();

      Choices = new List<WRegexBase>();
      Choices.Add(left);
      Choices.Add(right);
    }


    public ChoiceWRegex(IEnumerable<WRegexBase> choices)
    {
      Condition.Requires(choices, nameof(choices)).IsNotNull();
      Choices = new List<WRegexBase>(choices);
    }


    public void Add(WRegexBase choice)
    {
      Choices.Add(choice);
    }


    public override double CalculateSize()
    {
      return Choices.Max(c => c.CalculateSize());
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      List<NFAFragment> fragments = Choices.Select(c => c.CalculateNFAFragment(context)).ToList();
      NFANode s = NFANode.CreateSplit(context, fragments.Select(f => f.Start));
      return new NFAFragment(s, fragments.SelectMany(f => f.Out).ToList());
    }
  }
}
