using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public abstract class RuleModifier
  {
    public abstract void Invoke(StandardRule r);
  }


  public class ConditionRuleModifier : RuleModifier
  {
    Expression Expr;

    public ConditionRuleModifier(Expression expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      Expr = expr;
    }

    public override void Invoke(StandardRule r)
    {
      r.WithCondition(Expr);
    }
  }


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


  public class ScheduleRuleModifier : RuleModifier
  {
    int Seconds;

    public ScheduleRuleModifier(int seconds)
    {
      Seconds = seconds;
    }

    public override void Invoke(StandardRule r)
    {
      r.WithSchedule(TimeSpan.FromSeconds(Seconds));
    }
  }


#if false
  public class AnswerRuleModifier : RuleModifier
  {
    public AnswerRuleModifier(List<Rule> answerRules)
    {
    }

    public override void Invoke(Rule r)
    {
    }
  }
#endif
}
