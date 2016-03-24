using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class ErrorHandlingTests : TestHelper
  {
    [Test]
    public void CanExposeErrorsToCaller()
    {
      string s = @"
> Test
{{ Invalid line
";
      KnowledgeBase kb = new KnowledgeBase();
      Domain d = kb.NewDomain("Test");
      Assert.Throws<ParserException>(() => CfgParser.ParseConfigurationString(d, s));
    }
  }
}
