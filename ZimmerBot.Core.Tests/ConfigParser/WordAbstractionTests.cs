using NUnit.Framework;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class WordAbstractionTests : TestHelper
  {
    [Test]
    public void CanParseSingleWordAbstractions()
    {
      Domain d = ParseDomain(@"
! abstraction monday => weekday
");

      Assert.AreEqual(1, d.WordDefinitions.Count);
      Assert.AreEqual(1, d.WordDefinitions[0].Words.Count);
      Assert.AreEqual(1, d.WordDefinitions[0].Equivalents.Count);
      Assert.AreEqual("monday", d.WordDefinitions[0].Words[0]);
      Assert.AreEqual("weekday", d.WordDefinitions[0].Equivalents[0]);
    }


    [Test]
    public void CanParseMultiWordAbstractions()
    {
      Domain d = ParseDomain(@"
! abstraction monday,tuesday => weekday , day
");

      Assert.AreEqual(1, d.WordDefinitions.Count);
      Assert.AreEqual(2, d.WordDefinitions[0].Words.Count);
      Assert.AreEqual(2, d.WordDefinitions[0].Equivalents.Count);
      Assert.AreEqual("monday", d.WordDefinitions[0].Words[0]);
      Assert.AreEqual("tuesday", d.WordDefinitions[0].Words[1]);
      Assert.AreEqual("weekday", d.WordDefinitions[0].Equivalents[0]);
      Assert.AreEqual("day", d.WordDefinitions[0].Equivalents[1]);
    }
  }
}
