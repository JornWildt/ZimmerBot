using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  [TestFixture]
  public class WRegexMatchValueTests : TestHelper
  {
    [Test]
    public void CanEvaluateWordWRegex()
    {
      Trigger t = new Trigger(new WordWRegex("mosquito", "n"));
      WRegex.MatchResult result = CalculateMatchResult(t, "mosquito");
      Assert.AreEqual(1.0, result.Score);
      Assert.True(result.Matches.ContainsKey("n"));
      Assert.AreEqual("mosquito", result.Matches["n"]);
    }
  }
}
