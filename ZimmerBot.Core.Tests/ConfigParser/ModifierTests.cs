using System;
using System.Collections.Generic;
using NUnit.Framework;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class ModifierTests : TestHelper
  {
    [Test]
    public void CanCheckConditionWithNumber()
    {
      StandardRule r = ParseRule(@"
> aaa
! when session.responseCount = 0
: bbb");

      Assert.IsNotNull(r.Trigger.Condition);
      Assert.IsInstanceOf<BinaryOperatorExpr>(r.Trigger.Condition);
      BinaryOperatorExpr b = (BinaryOperatorExpr)r.Trigger.Condition;
      Assert.IsInstanceOf<ConstantValueExpr>(b.Right);
      ConstantValueExpr i = (ConstantValueExpr)b.Right;
      Assert.AreEqual(0, i.Value);
    }


    [Test]
    public void CanCheckConditionWithString()
    {
      StandardRule r = ParseRule(@"
> aaa
! when (xxx.yyy = ""a"")
: bbb");

      Assert.IsNotNull(r.Trigger.Condition);
      Assert.IsInstanceOf<BinaryOperatorExpr>(r.Trigger.Condition);
      BinaryOperatorExpr b = (BinaryOperatorExpr)r.Trigger.Condition;
      Assert.IsInstanceOf<ConstantValueExpr>(b.Right);
      ConstantValueExpr i = (ConstantValueExpr)b.Right;
      Assert.AreEqual("a", i.Value);
    }


    [Test]
    public void CanIncludeWeight()
    {
      StandardRule r = ParseRule(@"
> aaa
! weight 0.5
: bbb");

      IList<Reaction> reactions = CalculateReactions(r, "aaa");
      Assert.AreEqual(0.5, reactions[0].Score);
    }


#if false
    [Test]
    public void CanIncludeSchedule()
    {
      StandardRule r = ParseRule(@"
> aaa
! every 30
: Another 30 seconds.");

      Assert.IsInstanceOf<ScheduledTrigger>(r.Trigger);

      ScheduledTrigger trigger = (ScheduledTrigger)r.Trigger;
      Assert.IsNotNull(trigger.Schedule);
      Assert.AreEqual(TimeSpan.FromSeconds(30), trigger.Schedule.Value);
    }
#endif
  }
}