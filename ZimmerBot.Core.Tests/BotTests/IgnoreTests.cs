using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class IgnoreTests : TestHelper
  {
    [Test]
    public void CanIgnoreMatches()
    {
      BuildBot(@"
! ignore
{
  > aha | ah | hm | hmm | hmmm
}

> what is a bird
: Don't you know?

");

      AssertDialog("what is the dog", "???");
      AssertDialog("what is a bird", "Don't you know?");
      AssertDialog("hm what is a bird", "Don't you know?");
      AssertDialog("what is a hmmm bird", "Don't you know?");
      AssertDialog("what is a bird hm", "Don't you know?");
      AssertDialog("what ah is a bird", "Don't you know?");
      AssertDialog("what ah hm is a bird", "Don't you know?");
      AssertDialog("ah what ah hmm is a bird aha", "Don't you know?");
      AssertDialog("hmm aha what is a bird hmm hmmm", "Don't you know?");
      AssertDialog("hmm aha what is a DOG hmm hmmm", "???");
    }
  }
}
