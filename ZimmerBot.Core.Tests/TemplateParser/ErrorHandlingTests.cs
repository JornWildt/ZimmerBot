using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Tests.TemplateParser
{
  [TestFixture]
  public class ErrorHandlingTests : TestHelper
  {
    [Test]
    public void CanExposeErrorsToCaller()
    {
      try
      {
        SequenceTemplateToken tokens = ParseTemplate(@"aaa <<");
        Assert.Fail("Missing exception");
      }
      catch (ParserException ex)
      {
        Assert.AreEqual(1, ex.Errors.Count);
        Assert.AreEqual(1, ex.Errors[0].LineNo);
        Assert.AreEqual(5, ex.Errors[0].Position);
        StringAssert.EndsWith("(1,5)", ex.Message);
      }
    }
  }
}
