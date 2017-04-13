using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class MultiTriggerRuleTests : TestHelper
  {
    [Test]
    public void CanHaveMultipleTriggers()
    {
      string cfg = @"
> xxx
> yyy
: AAA
";
      BuildBot(cfg);

      AssertDialog("xxx", "AAA");
      AssertDialog("yyy", "AAA");
    }
  }
}
