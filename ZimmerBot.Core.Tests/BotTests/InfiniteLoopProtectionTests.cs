using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class InfiniteLoopProtectionTests : TestHelper
  {
    [Test]
    public void CanBreakContinue()
    {
      BuildBot(@"
> hi
! continue (""hi"")
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
