﻿using System.Collections.Generic;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class ConceptTests : TestHelper
  {
    KnowledgeBase KB;

    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      KB = new KnowledgeBase();
      KB.LoadFromFiles("ConfigParser", "ConceptTests.zbot");
    }


    [TestCase("I like bananas", "I like bananas too")]
    [TestCase("I enjoy bananas", "I enjoy bananas too")]
    [TestCase("I prefer apple", "I prefer apple too")]
    [TestCase("I like chicken", "I like chicken too")]
    [TestCase("I really like chicken", "I like chicken too")]
    [TestCase("I LIKE chiCKEN", "I LIKE chiCKEN too")] // FIXME: improve casing on match ...
    public void CanMatchConcepts(string input, string expectedAnswer)
    {
      string output = GetResponseFor(input);
      Assert.AreEqual(expectedAnswer, output);
    }


    protected string GetResponseFor(string s)
    {
      EvaluationContext context = BuildEvaluationContextFromInput(s);
      SessionState state = new SessionState();
      var sessionState = new Dictionary<string, dynamic>();// FIXME: use common code
      sessionState[Constants.LineCountKey] = 0d;
      state[Constants.SessionStoreKey] = sessionState;


      Response response = BotUtility.Invoke(KB, new Request { Input = s });
      Assert.AreEqual(1, response.Output.Length);
      return response.Output[0];
    }
  }
}
