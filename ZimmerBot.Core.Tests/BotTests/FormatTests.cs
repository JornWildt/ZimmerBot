using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class FormatTests : TestHelper
  {
    [Test]
    public void CanUpperCaseFirstLetter()
    {
      BuildBot(@"
> Hello +
: Hi <1; format=""UF"">
");
      AssertDialog("Hello donald", "Hi Donald");
    }
  }
}
