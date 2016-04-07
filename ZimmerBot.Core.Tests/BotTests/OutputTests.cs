using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class OutputTests : TestHelper
  {
    [Test]
    public void CanTestForUndefinedValueInOutputTemplates()
    {
      BuildBot(@"
> aaa
: Value is <if(session.x)>SET<else>UNSET<endif>

> bbb
: OK
! set session.x = 1
");

      AssertDialog("aaa", "Value is UNSET");
      AssertDialog("bbb", "OK");
      AssertDialog("aaa", "Value is SET");
    }
  }
}
