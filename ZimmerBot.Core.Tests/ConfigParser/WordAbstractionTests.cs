using NUnit.Framework;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class WordConceptTests : TestHelper
  {
    [Test]
    public void CanParseSingleWordConcepts()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! concept weekday = monday
");

      Assert.AreEqual(1, kb.Concepts.Count);
      Assert.AreEqual(1, kb.Concepts["weekday"].OriginalWords.Count);
      Assert.AreEqual("weekday", kb.Concepts["weekday"].Name);
      Assert.AreEqual("monday", kb.Concepts["weekday"].OriginalWords[0]);
    }


    [Test]
    public void CanParseMultiWordConcepts()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! concept weekday = monday,tuesday
");

      Assert.AreEqual(1, kb.Concepts.Count);
      Assert.AreEqual(2, kb.Concepts["weekday"].OriginalWords.Count);
      Assert.AreEqual("weekday", kb.Concepts["weekday"].Name);
      Assert.AreEqual("monday", kb.Concepts["weekday"].OriginalWords[0]);
      Assert.AreEqual("tuesday", kb.Concepts["weekday"].OriginalWords[1]);
    }
  }
}
