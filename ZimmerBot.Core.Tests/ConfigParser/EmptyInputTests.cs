using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class EmptyInputTests : TestHelper
  {
    [Test]
    public void CanHandleEmptyInputPatternsForStartupRules()
    {
      StandardRule r = ParseRule(@"
>
: Startup");

      Assert.IsNull(r.Trigger.Regex);
    }
  }
}
