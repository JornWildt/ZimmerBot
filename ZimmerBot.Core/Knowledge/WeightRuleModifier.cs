namespace ZimmerBot.Core.Knowledge
{
  public class WeightRuleModifier : RuleModifier
  {
    double Weight;

    public WeightRuleModifier(double weight)
    {
      Weight = weight;
    }

    public override void Invoke(Executable e)
    {
      e.WithWeight(Weight);
    }
  }
}
