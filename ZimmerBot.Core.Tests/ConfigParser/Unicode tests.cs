using NUnit.Framework;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class Unicode_tests : TestHelper
  {
    [Test]
    public void CanLoadAndMatchUnicodeCharacters()
    {
      KnowledgeBase kb = KnowledgeBase.CreateFromFiles("ConfigParser", "UnicodeTest.zbot");
      EvaluationContext context = BuildEvaluationContextFromInput("Über Åsen");
      ReactionSet reactions = kb.FindMatchingReactions(context);

      Assert.AreEqual(1, reactions.Count);
      string response = reactions[0].GenerateResponse();
      Assert.AreEqual("Østers: Über", response);
    }


    [Test]
    public void CanUseUnicodeInFunctions()
    {
      KnowledgeBase kb = KnowledgeBase.CreateFromFiles("ConfigParser", "UnicodeTest.zbot");
      EvaluationContext context = BuildEvaluationContextFromInput("ÆØÅ");
      ReactionSet reactions = kb.FindMatchingReactions(context);

      Assert.AreEqual(1, reactions.Count);
      string response = reactions[0].GenerateResponse();
      Assert.AreEqual("Got: 'ÆØÅ'", response);
    }
  }
}
