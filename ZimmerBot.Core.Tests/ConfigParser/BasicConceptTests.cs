using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class BasicConceptTests : TestHelper
  {
    [Test]
    public void CanParseSingleWordConcepts()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! concept weekday = monday
");

      Assert.AreEqual(1, kb.Concepts.Count);
      Assert.AreEqual(1, kb.Concepts["weekday"].Choices.Choices.Count);
      Assert.AreEqual("weekday", kb.Concepts["weekday"].Name);
      Assert.AreEqual("monday", ((LiteralWRegex)kb.Concepts["weekday"].Choices.Choices[0]).Literal);
    }


    [Test]
    public void CanParseMultiWordConcepts()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! concept weekday = monday,tuesday
");

      Assert.AreEqual(1, kb.Concepts.Count);
      Assert.AreEqual(2, kb.Concepts["weekday"].Choices.Choices.Count);
      Assert.AreEqual("weekday", kb.Concepts["weekday"].Name);
      Assert.AreEqual("monday", ((LiteralWRegex)kb.Concepts["weekday"].Choices.Choices[0]).Literal);
      Assert.AreEqual("tuesday", ((LiteralWRegex)kb.Concepts["weekday"].Choices.Choices[1]).Literal);
    }
  }
}
