using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      KB = KnowledgeBase.CreateFromFiles("ConfigParser", "ConceptTests.zbot");
    }


    [TestCase("I like bananas", "I like bananas too")]
    [TestCase("I enjoy bananas", "I enjoy bananas too")]
    [TestCase("I prefer apple", "I prefer apple too")]
    public void CanMatchConcepts(string input, string expectedAnswer)
    {
      string output = GetResponseFor(input);
      Assert.AreEqual(expectedAnswer, output);
    }


    protected string GetResponseFor(string s)
    {
      EvaluationContext context = BuildEvaluationContextFromInput(s);
      BotState state = new BotState();
      state["state.conversation.entries.Count"] = 0d; // FIXME: use common code

      Response response = BotUtility.Invoke(KB, state, new Request { Input = s });
      Assert.AreEqual(1, response.Output.Length);
      return response.Output[0];
    }
  }
}
