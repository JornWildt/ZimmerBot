using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class InfiniteLoopProtectionTests : TestHelper
  {
    [Test]
    public void CanBreakContinueWith()
    {
      BuildBot(@"
> hi
! continue with hi
");
      Assert.Throws<RepetitionException>(() => AssertDialog("hi", "x"));
    }


    [Test]
    public void CanBreakContinueAt()
    {
      BuildBot(@"
> hi
! continue at aaa

<aaa>
: iiiii
! continue at aaa
");
      Assert.Throws<RepetitionException>(() => AssertDialog("hi", "x"));
    }


    [Test]
    public void CanBreakContinueEmpty()
    {
      BuildBot(@"
> hi
! continue

> *
! continue with hi
");
      Assert.Throws<RepetitionException>(() => AssertDialog("hi", "x"));
    }


    [Test]
    public void CanBreakRecursion()
    {
      BuildBot(@"
> hello
: say <<@ hello>>
");
      Assert.Throws<RepetitionException>(() => AssertDialog("hello", "x"));
    }
  }
}
