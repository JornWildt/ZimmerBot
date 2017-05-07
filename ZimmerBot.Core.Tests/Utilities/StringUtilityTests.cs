using NUnit.Framework;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Tests.Utilities
{
  [TestFixture]
  public class StringUtilityTests : TestHelper
  {
    [TestCase("aa", "aa")]
    [TestCase("aa bb", "aa_bb")]
    [TestCase("aa0 11", "aa0_11")]
    [TestCase("æbc æoå 11 by", "æbc_æoå_11_by")]
    [TestCase("aa:bb", "aa_bb")]
    [TestCase("aa/bb", "aa_bb")]
    [TestCase("aa b/b", "aa_b_b")]
    [TestCase("aa b:&&%b", "aa_b____b")]
    public void CanBuildIdentifierFromWord(string word, string expectedId)
    {
      string id = StringUtility.Word2Identifier(word);
      Assert.AreEqual(expectedId, id);
    }
  }
}
