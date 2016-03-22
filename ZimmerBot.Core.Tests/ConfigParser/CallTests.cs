using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class CallTests : TestHelper
  {
    [Test]
    public void CanParseCallStatement()
    {
      Rule r = ParseRule(@"
> aaa
! call X.Y(a,""k"")
: bbb
");

      Assert.IsNotNull(r.OutputStatements);
      Assert.AreEqual(2, r.OutputStatements.Count);
      Assert.IsInstanceOf<CallOutputStatment>(r.OutputStatements[0]);
      Assert.IsInstanceOf<TemplateOutputStatement>(r.OutputStatements[1]);
    }
  }
}
