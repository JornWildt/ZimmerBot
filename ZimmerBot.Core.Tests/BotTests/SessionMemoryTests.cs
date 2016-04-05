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
  public class SessionMemoryTests : TestHelper
  {
    [Test]
    public void CanStoreInSession()
    {
      BuildBot(@"
! concept color = red, green, blue
> The moon is %color
! set session.moonColor = $color
: OK

> What color is the moon
: The moon is <session.moonColor>
");

      string r1 = Invoke("The moon is blue");
      Assert.AreEqual("OK", r1);

      string r2 = Invoke("What color is the moon?");
      Assert.AreEqual("The moon is blue", r2);
    }


    [Test]
    public void SessionsDoesNotShareState()
    {
      BuildBot(@"
! concept color = red, green, blue
> The moon is %color
! set session.moonColor = $color
: OK: <color>

> What color is the moon
: The moon is <session.moonColor>
");

      string r1a = Invoke(B, new Request("session1", "default") { Input = "The moon is blue" });
      Assert.AreEqual("OK: blue", r1a);

      string r1b = Invoke(B, new Request("session2", "default") { Input = "The moon is green" });
      Assert.AreEqual("OK: green", r1b);

      string r2a = Invoke(B, new Request("session1", "default") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is blue", r2a);

      string r2b = Invoke(B, new Request("session2", "default") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is green", r2b);
    }
  }
}
