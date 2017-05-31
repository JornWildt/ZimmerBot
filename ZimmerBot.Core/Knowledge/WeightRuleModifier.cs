namespace ZimmerBot.Core.Knowledge
{
  public class WeightRuleModifier : RuleModifier
  {
    double Weight;

    public WeightRuleModifier(double weight)
    {
      Weight = weight;
    }

    public override void Invoke(StandardRule r)
    {
      r.WithWeight(Weight);
    }
  }
}
