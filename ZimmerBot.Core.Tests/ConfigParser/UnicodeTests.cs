using System.Linq;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class UnicodeTests : TestHelper
  {
    [Test]
    public void CanLoadAndMatchUnicodeCharacters()
    {
      KnowledgeBase kb = new KnowledgeBase();
      kb.LoadFromFiles("ConfigParser", "UnicodeTests.zbot");
      EvaluationContext context = BuildEvaluationContextFromInput("Über Åsen");
      ReactionSet reactions = kb.FindMatchingReactions(context);

      Assert.AreEqual(1, reactions.Count);
      string response = reactions[0].GenerateResponse().Aggregate((a, b) => a + "\n" + b);
      Assert.AreEqual("Østers: Über", response);
    }


    [Test]
    public void CanUseUnicodeInFunctions()
    {
      KnowledgeBase kb = new KnowledgeBase();
      kb.LoadFromFiles("ConfigParser", "UnicodeTests.zbot");
      EvaluationContext context = BuildEvaluationContextFromInput("ÆØÅ");
      ReactionSet reactions = kb.FindMatchingReactions(context);

      Assert.AreEqual(1, reactions.Count);
      string response = reactions[0].GenerateResponse().Aggregate((a, b) => a + "\n" + b);
      Assert.AreEqual("Got: 'ÆØÅ'", response);
    }
  }
}
