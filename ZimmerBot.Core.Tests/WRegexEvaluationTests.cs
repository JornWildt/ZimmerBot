﻿using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  [TestFixture]
  public class WRegexEvaluationTests : TestHelper
  {
    [Test]
    public void CanEvaluateWordWRegex()
    {
      Trigger t = new Trigger("mosquito");
      Assert.AreEqual(0, CalculateScore(t, "Test"));
      Assert.AreEqual(1, CalculateScore(t, "mosquito"));
      Assert.AreEqual(1, CalculateScore(t, "MOSQUITO"));
    }


    [Test]
    public void CanEvaluateWordSequenceWRegex()
    {
      Trigger t = new Trigger("I", "am", "happy");
      Assert.AreEqual(0, CalculateScore(t, "Test"));
      Assert.AreEqual(3, CalculateScore(t, "I am happy"));
      Assert.AreEqual(0, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0, CalculateScore(t, "I happy am"));
      Assert.AreEqual(0, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateWildcardSequenceWRegex()
    {
      Trigger t = new Trigger("I", new WildcardWRegex(), "happy");
      Assert.AreEqual(0.0, CalculateScore(t, "Test"));
      Assert.AreEqual(3.0, CalculateScore(t, "I am happy"));
      Assert.AreEqual(3.0, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0, CalculateScore(t, "I happy am"));
      Assert.AreEqual(0, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateDoubleWildcardSequenceWRegex()
    {
      Trigger t = new Trigger("I", new WildcardWRegex(), new WildcardWRegex(), "happy");
      Assert.AreEqual(0.0, CalculateScore(t, "Test"));
      Assert.AreEqual(0, CalculateScore(t, "I am happy"));
      Assert.AreEqual(0, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0, CalculateScore(t, "I happy am"));
      Assert.AreEqual(4.0, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateWordRepitionWRegex()
    {
      Trigger t = new Trigger("Run", new RepetitionWRegex(new WordWRegex("very")), "fast");
      Assert.AreEqual(0, CalculateScore(t, "Test"));
      Assert.AreEqual(0, CalculateScore(t, "very"));
      Assert.AreEqual(0, CalculateScore(t, "Run"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very very fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very very very fast"));
    }


    [Test]
    public void CanEvaluateWildcardRepitionWRegex()
    {
      Trigger t = new Trigger("Run", new RepetitionWRegex(new WildcardWRegex()), "fast");
      Assert.AreEqual(0, CalculateScore(t, "Test"));
      Assert.AreEqual(0, CalculateScore(t, "very"));
      Assert.AreEqual(0, CalculateScore(t, "Run"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very very fast"));
      Assert.AreEqual(3.0, CalculateScore(t, "Run very very very fast"));
    }

    [Test]
    public void CanEvaluateDoubleWildcardRepetition()
    {
      Trigger t = new Trigger(new RepetitionWRegex(new WildcardWRegex()), "mother", new RepetitionWRegex(new WildcardWRegex()));
      Assert.AreEqual(3, CalculateScore(t, "I miss my mother"));
      Assert.AreEqual(3, CalculateScore(t, "I miss my mother so much"));
      Assert.AreEqual(3, CalculateScore(t, "mother is the best"));
      Assert.AreEqual(0, CalculateScore(t, "father is the best"));
    }

    [Test]
    public void CanEvaluateChoiceWRegex()
    {
      Trigger t = new Trigger("She", new ChoiceWRegex(new WordWRegex("sleeps"), new WordWRegex("walks")), "today");
      Assert.AreEqual(0, CalculateScore(t, "Test"));
      Assert.AreEqual(0, CalculateScore(t, "she today"));
      Assert.AreEqual(0, CalculateScore(t, "she runs today"));
      Assert.AreEqual(3.0, CalculateScore(t, "she sleeps today"));
      Assert.AreEqual(3.0, CalculateScore(t, "she walks today"));
    }

    //[Test]
    //public void CanEvaluateNegatedWRegex()
    //{
    //  Trigger t = new Trigger(new NegationWRegex(new WordWRegex("aaa")));
    //  Assert.AreEqual(0, CalculateScore(t, "aaa"));
    //  Assert.AreEqual(0.9, CalculateScore(t, "bbb"));
    //}
  }
}
