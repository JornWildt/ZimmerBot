namespace ZimmerBot.Core.Knowledge
{
  public class ScheduleRuleModifier : RuleModifier
  {
    int Seconds;

    public ScheduleRuleModifier(int seconds)
    {
      Seconds = seconds;
    }


    public override void Invoke(StandardRule r)
    {
      // FIXME!
      //r.WithSchedule(TimeSpan.FromSeconds(Seconds));
    }
  }
}
