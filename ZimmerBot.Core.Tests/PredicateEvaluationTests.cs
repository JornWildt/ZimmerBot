using System;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  [TestFixture]
  public class PredicateEvaluationTests : TestHelper
  {
    [Test]
    public void CanEvaluateWordWRegex()
    {
      Trigger t = new Trigger("mosquito");
      Assert.AreEqual(0.1, CalculateScore(t, "Test"));
      Assert.AreEqual(1, CalculateScore(t, "mosquito"));
      Assert.AreEqual(1, CalculateScore(t, "MOSQUITO"));
    }


    [Test]
    public void CanEvaluateWordSequenceWRegex()
    {
      Trigger t = new Trigger("I", "am", "happy");
      Assert.AreEqual(0.001, CalculateScore(t, "Test"));
      Assert.AreEqual(1, CalculateScore(t, "I am happy"));
      Assert.AreEqual(0.1, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0.25, CalculateScore(t, "I happy am"));
      Assert.AreEqual(0.5, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateWildcardSequenceWRegex()
    {
      Trigger t = new Trigger("I", new WildcardWRegex(), "happy");
      Assert.AreEqual(0.0, CalculateScore(t, "Test"));
      Assert.AreEqual(1.0, CalculateScore(t, "I am happy"));
      Assert.AreEqual(1.0, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0.5, CalculateScore(t, "I happy am"));
      Assert.AreEqual(0.5, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateDoubleWildcardSequenceWRegex()
    {
      Trigger t = new Trigger("I", new WildcardWRegex(), new WildcardWRegex(), "happy");
      Assert.AreEqual(0.0, CalculateScore(t, "Test"));
      Assert.AreEqual(0.5, CalculateScore(t, "I am happy"));
      Assert.AreEqual(0.5, CalculateScore(t, "I very happy"));
      Assert.AreEqual(0.25, CalculateScore(t, "I happy am"));
      Assert.AreEqual(1.0, CalculateScore(t, "I am very happy"));
    }


    [Test]
    public void CanEvaluateWordRepitionWRegex()
    {
      Trigger t = new Trigger("Run", new RepitionWRegex(new WordWRegex("very")), "fast");
      Assert.AreEqual(0.01, CalculateScore(t, "Test"));
      Assert.AreEqual(0.01, CalculateScore(t, "very"));
      Assert.AreEqual(0.1, CalculateScore(t, "Run"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very very fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very very very fast"));
    }


    [Test]
    public void CanEvaluateWildcardRepitionWRegex()
    {
      Trigger t = new Trigger("Run", new RepitionWRegex(new WildcardWRegex()), "fast");
      Assert.AreEqual(0.01, CalculateScore(t, "Test"));
      Assert.AreEqual(0.01, CalculateScore(t, "very"));
      Assert.AreEqual(0.1, CalculateScore(t, "Run"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very very fast"));
      Assert.AreEqual(1.0, CalculateScore(t, "Run very very very fast"));
    }


    [Test]
    public void CanEvaluateChoiceWRegex()
    {
      Trigger t = new Trigger("She", new ChoiceWRegex(new WordWRegex("sleeps"), new WordWRegex("walks")), "today");
      Assert.AreEqual(0.001, CalculateScore(t, "Test"));
      Assert.AreEqual(0.05, CalculateScore(t, "she today"));
      Assert.AreEqual(0.1, CalculateScore(t, "she runs today"));
      Assert.AreEqual(1.0, CalculateScore(t, "she sleeps today"));
      Assert.AreEqual(1.0, CalculateScore(t, "she walks today"));
    }


    protected double CalculateScore(Trigger t, string text)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence sequence = tokenizer.Tokenize(text);
      ZTokenSequence input = sequence.Statements[0];

      BotState state = new BotState();
      EvaluationContext context = new EvaluationContext(state, input);
      double score = t.CalculateTriggerScore(context);

      Console.WriteLine("Score for '{0}' = {1}.", text, score);
      return Math.Round(score, 4);
    }
  }
}


//KnowledgeBase kb = new KnowledgeBase();
//Domain d = kb.NewDomain("Test");
