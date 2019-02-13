using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ConceptOutputTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      BuildBot(@"
! concept fruit = bananas, apple, strawberry

> what do you like
: I like <(%fruit)>
");
    }


    [Test]
    public void CanUseConceptInOutput()
    {
      Assert.Pass();
    }
  }
}
