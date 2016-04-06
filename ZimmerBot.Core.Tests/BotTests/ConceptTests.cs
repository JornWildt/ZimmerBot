using System.Collections.Generic;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ConceptTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      KnowledgeBase kb = new KnowledgeBase();
      kb.LoadFromFiles("ConfigParser", "ConceptTests.zbot");
      B = new Bot(kb);
    }


    [TestCase("I like bananas", "I like bananas too")]
    [TestCase("I enjoy bananas", "I enjoy bananas too")]
    [TestCase("I prefer apple", "I prefer apple too")]
    [TestCase("I like chicken", "I like chicken too")]
    [TestCase("I really like chicken", "I like chicken too")]
    [TestCase("I LIKE chiCKEN", "I LIKE chiCKEN too")] // FIXME: improve casing on match ...
    public void CanMatchConcepts(string input, string expectedAnswer)
    {
      string output = Invoke(input);
      Assert.AreEqual(expectedAnswer, output);
    }


    //protected string GetResponseFor(string s)
    //{
    //  EvaluationContext context = BuildEvaluationContextFromInput(s);
    //  RequestState state = new RequestState();
    //  var sessionState = new Dictionary<string, dynamic>();// FIXME: use common code
    //  sessionState[Constants.LineCountKey] = 0d;
    //  state[Constants.SessionStoreKey] = sessionState;


    //  Response response = BotUtility.Invoke(KB, new Request { Input = s });
    //  Assert.AreEqual(1, response.Output.Length);
    //  return response.Output[0];
    //}
  }
}
