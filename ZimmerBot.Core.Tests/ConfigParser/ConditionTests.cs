using NUnit.Framework;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class ConditionTests : TestHelper
  {
    [Test]
    public void CanCheckConditionWithNumber()
    {
      Rule r = ParseRule(@"
> aaa
& (state.conversation.entries.Count = 0)
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
  }
}
