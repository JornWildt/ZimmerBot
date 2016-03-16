using System;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Tests
{
  public class TestHelper
  {
    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    //[OneTimeSetUp]
    [TestFixtureSetUp]
    public void MasterTestFixtureSetUp()
    {
      TestFixtureSetUp();
    }


    /// <summary>
    /// Executed only once before all tests. Override in subclasses to do subclass
    /// set up. Remember to call base.TestFixtureSetUp().
    /// NOTE: The [TestFixtureSetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void TestFixtureSetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [SetUp]
    public void MasterSetUp()
    {
      SetUp();
    }


    /// <summary>
    /// Executed before each test method is run. Override in subclasses to do subclass
    /// set up. Remember to call base.SetUp().
    /// NOTE: The [SetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void SetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TearDown]
    public void MasterTearDown()
    {
      TearDown();
    }

    /// <summary>
    /// Executed after each test method is run.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TearDown().
    /// NOTE: [TearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TearDown()
    {
    }

    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    //[OneTimeTearDown]
    [TestFixtureTearDown]
    public void TestFixtureMasterTearDown()
    {
      TestFixtureTearDown();
    }

    /// <summary>
    /// Executed only once after all tests.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TestFixtureTearDown().
    /// NOTE: [TestFixtureTearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TestFixtureTearDown()
    {
    }


    protected WRegex.MatchResult CalculateMatchResult(Trigger t, string text)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence sequence = tokenizer.Tokenize(text);
      ZTokenSequence input = sequence.Statements[0];

      BotState state = new BotState();
      EvaluationContext context = new EvaluationContext(state, input, null, executeScheduledRules: false);
      WRegex.MatchResult result = t.CalculateTriggerScore(context);
      return result;
    }


    protected double CalculateScore(Trigger t, string text)
    {
      WRegex.MatchResult result = CalculateMatchResult(t, text);
      Console.WriteLine("Score for '{0}' = {1}.", text, result.Score);
      return Math.Round(result.Score, 4);
    }
  }
}
