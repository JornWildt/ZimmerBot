using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class UserMemoryTests : TestHelper
  {
    [Test]
    public void CanStoreWithUser()
    {
      BuildBot(@"
! concept color = red, green, blue
> The moon is %color
! set user.moonColor = $color
: OK

> What color is the moon
: The moon is <user.moonColor>
");

      string r1 = Invoke("The moon is red");
      Assert.AreEqual("OK", r1);

      string r2 = Invoke("What color is the moon?");
      Assert.AreEqual("The moon is red", r2);
    }
  }
}
