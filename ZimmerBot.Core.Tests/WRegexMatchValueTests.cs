using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  [TestFixture]
  public class WRegexMatchValueTests : TestHelper
  {
    [Test]
    public void CanMatchValuesForWordWRegex()
    {
      Trigger t = new Trigger(new WordWRegex("mosquito", "n"));

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquito");
      Assert.AreEqual(1.0, result.Score);
      AssertMatchesContain(result, "n", "mosquito");

      result = CalculateMatchResult(t, "MOSQUITO");
      Assert.AreEqual(1.0, result.Score);
      Assert.False(result.Matches.ContainsKey("mosquito"));
      AssertMatchesContain(result, "n", "MOSQUITO");
    }


    [Test]
    public void CanMatchValuesForWildcardWRegex()
    {
      Trigger t = new Trigger(new WildcardWRegex("w"));

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquito");
      Assert.AreEqual(1.0, result.Score);
      AssertMatchesContain(result, "w", "mosquito");
    }


    [Test]
    public void CanMatchValuesForSequenceWRegex()
    {
      Trigger t = new Trigger(new WordWRegex("mosquitos", "n"), new WordWRegex("fly", "f"));

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquitos");
      Assert.AreEqual(0.2, result.Score);
      AssertMatchesContain(result, "n", "mosquitos");

      result = CalculateMatchResult(t, "MOSQUITOS");
      Assert.AreEqual(0.2, result.Score);
      AssertMatchesContain(result, "n", "MOSQUITOS");
      Assert.False(result.Matches.ContainsKey("f"));

      result = CalculateMatchResult(t, "mosquitos fly");
      Assert.AreEqual(2, result.Score);
      AssertMatchesContain(result, "n", "mosquitos");
      AssertMatchesContain(result, "f", "fly");
    }


    [Test]
    public void CanMatchValuesForChoiceWRegex()
    {
      Trigger t = new Trigger(new ChoiceWRegex(new WordWRegex("mosquitos", "n"), new WordWRegex("fly", "f")));

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquitos");
      Assert.AreEqual(1, result.Score);
      AssertMatchesContain(result, "n", "mosquitos");
      Assert.False(result.Matches.ContainsKey("f"));

      result = CalculateMatchResult(t, "fly");
      Assert.AreEqual(1, result.Score);
      AssertMatchesContain(result, "f", "fly");
      Assert.False(result.Matches.ContainsKey("n"));

      result = CalculateMatchResult(t, "zimmer");
      Assert.AreEqual(0.1, result.Score);
      Assert.False(result.Matches.ContainsKey("n"));
      Assert.False(result.Matches.ContainsKey("f"));
    }


    [Test]
    public void CanMatchValuesForRepetitionWRegex()
    {
      Trigger t = new Trigger("mosquito", new RepetitionWRegex(new WordWRegex("and", "a"), "b"), "bee");

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquito bee");
      Assert.AreEqual(3, result.Score);
      Assert.False(result.Matches.ContainsKey("a"));
      AssertMatchesContain(result, "b", "");

      result = CalculateMatchResult(t, "mosquito and bee");
      Assert.AreEqual(3, result.Score);
      AssertMatchesContain(result, "a", "and");
      AssertMatchesContain(result, "b", "and");

      result = CalculateMatchResult(t, "mosquito and and bee");
      Assert.AreEqual(3, result.Score);
      AssertMatchesContain(result, "a", "and");
      AssertMatchesContain(result, "b", "and and");
    }


    [Test]
    public void CanMatchValuesForEndRepetitionWRegex()
    {
      Trigger t = new Trigger("mosquito", new RepetitionWRegex(new WordWRegex("and", "a"), "b"));

      WRegex.MatchResult result = CalculateMatchResult(t, "mosquito");
      Assert.AreEqual(2, result.Score);
      Assert.False(result.Matches.ContainsKey("a"));
      AssertMatchesContain(result, "b", "");

      result = CalculateMatchResult(t, "mosquito bee");
      Assert.AreEqual(2, result.Score);
      Assert.False(result.Matches.ContainsKey("a"));
      AssertMatchesContain(result, "b", "");

      result = CalculateMatchResult(t, "mosquito and");
      Assert.AreEqual(2, result.Score);
      AssertMatchesContain(result, "a", "and");
      AssertMatchesContain(result, "b", "and");
    }


    private void AssertMatchesContain(WRegex.MatchResult result, string key, object value)
    {
      Assert.True(result.Matches.ContainsKey(key));
      Assert.AreEqual(value, result.Matches[key]);
    }
  }
}
