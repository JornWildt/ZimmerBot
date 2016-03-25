using System;
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
      Rule r = ParseRule(@"
> aaa
& state.conversation.entries.Count = 0
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
      Rule r = ParseRule(@"
> aaa
& (xxx.yyy = ""a"")
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
      Rule r = ParseRule(@"
> aaa
* 0.5
: bbb");

      Reaction reaction = CalculateReaction(r, "aaa");
      Assert.AreEqual(0.5, reaction.Score);
    }


    [Test]
    public void CanIncludeSchedule()
    {
      Rule r = ParseRule(@"
> aaa
! every 30");

      Assert.IsNotNull(r.Trigger.Schedule);
      Assert.AreEqual(TimeSpan.FromSeconds(30), r.Trigger.Schedule.Value);
    }
  }
}