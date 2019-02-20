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
    public void CanCheckConditionWithTimespan()
    {
      StandardRule r = ParseRule(@"
> aaa
! when 00:10:55
: bbb");

      Assert.IsNotNull(r.Trigger.Condition);
      Assert.IsInstanceOf<ConstantValueExpr>(r.Trigger.Condition);
      ConstantValueExpr v = (ConstantValueExpr)r.Trigger.Condition;
      Assert.AreEqual(new TimeSpan(0, 10, 55), v.Value);
    }


    [Test]
    public void CanCheckConditionWithFunctionCall()
    {
      StandardRule r = ParseRule(@"
> aaa
! when silent(00:30:10)
: bbb");

      Assert.IsNotNull(r.Trigger.Condition);
      Assert.IsInstanceOf<FunctionCallExpr>(r.Trigger.Condition);
      FunctionCallExpr func = (FunctionCallExpr)r.Trigger.Condition;

      Assert.AreEqual("silent", func.FunctionName);
      ConstantValueExpr v = (ConstantValueExpr)func.Parameters[0];
      Assert.AreEqual(new TimeSpan(0, 30, 10), v.Value);
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
  }
}