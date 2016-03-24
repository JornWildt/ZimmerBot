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
      try
      {
        CfgParser.ParseConfigurationString(d, s);
        Assert.Fail("Missing exception");
      }
      catch (ParserException ex)
      {
        Assert.AreEqual("string input", ex.Filename);
        Assert.AreEqual(1, ex.Errors.Count);
        Assert.AreEqual(3 , ex.Errors[0].LineNo);
        Assert.AreEqual(1, ex.Errors[0].Position);
        StringAssert.EndsWith("(3,1)", ex.Message);
      }
    }
  }
}
