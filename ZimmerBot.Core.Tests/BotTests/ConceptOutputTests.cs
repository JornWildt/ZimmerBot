using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ConceptOutputTests : TestHelper
  {
    [Test]
    public void CanUseConceptInOutput()
    {
      BuildBot(@"
! concept fruit = bananas, apple

> fruit
: I like ""<(%fruit)>""
");
      bool[] matches = new bool[2];
      for (int i = 0; i < 10; ++i)
      {
        string output = Invoke("fruit");

        if (output == "I like \"bananas\"")
          matches[0] = true;
        else if (output == "I like \"apple\"")
          matches[1] = true;
        else
          Assert.Fail("Unexpected output: " + output);
      }

      Assert.True(matches[0]);
      Assert.True(matches[1]);
    }
  }
}
