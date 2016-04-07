using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class BotMemoryTests : TestHelper
  {
    [Test]
    public void CanStoreInBot()
    {
      BuildBot(@"
! concept color = red, green, blue
> The moon is %color
! set bot.moonColor = $color
: OK

> What color is the moon
: The moon is <bot.moonColor>
");

      string r1 = Invoke("The moon is blue");
      Assert.AreEqual("OK", r1);

      string r2 = Invoke("What color is the moon?");
      Assert.AreEqual("The moon is blue", r2);
    }
  }
}
