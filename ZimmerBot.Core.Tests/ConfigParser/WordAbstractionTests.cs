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
      Domain d = ParseDomain(@"
! concept weekday = monday
");

      Assert.AreEqual(1, d.Concepts.Count);
      Assert.AreEqual(1, d.Concepts[0].Words.Count);
      Assert.AreEqual("weekday", d.Concepts[0].Name);
      Assert.AreEqual("monday", d.Concepts[0].Words[0]);
    }


    [Test]
    public void CanParseMultiWordConcepts()
    {
      Domain d = ParseDomain(@"
! concept weekday = monday,tuesday
");

      Assert.AreEqual(1, d.Concepts.Count);
      Assert.AreEqual(2, d.Concepts[0].Words.Count);
      Assert.AreEqual("weekday", d.Concepts[0].Name);
      Assert.AreEqual("monday", d.Concepts[0].Words[0]);
      Assert.AreEqual("tuesday", d.Concepts[0].Words[1]);
    }
  }
}
