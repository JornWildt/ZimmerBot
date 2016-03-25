﻿using System;
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
    public abstract void Invoke(Rule r);
  }


  public class ConditionRuleModifier : RuleModifier
  {
    Expression Expr;

    public ConditionRuleModifier(Expression expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      Expr = expr;
    }

    public override void Invoke(Rule r)
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

    public override void Invoke(Rule r)
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

    public override void Invoke(Rule r)
    {
      r.WithSchedule(TimeSpan.FromSeconds(Seconds));
    }
  }
}
