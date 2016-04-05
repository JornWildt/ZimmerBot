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


    [Test]
    public void SessionsDoesShareUserState()
    {
      string cfg = @"
! concept color = red, green, blue
> The moon is %color
! set user.moonColor = $color
: OK: <color>

> What color is the moon
: The moon is <user.moonColor>
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      Bot b1 = new Bot(kb);
      Bot b2 = new Bot(kb);

      string r1a = Invoke(b1, new Request("session1", "default") { Input = "The moon is blue" });
      Assert.AreEqual("OK: blue", r1a);

      string r2a = Invoke(b1, new Request("session1", "default") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is blue", r2a);

      string r2b = Invoke(b2, new Request("session2", "default") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is blue", r2b);
    }


    [Test]
    public void UsersDoNotShareUserState()
    {
      string cfg = @"
! concept color = red, green, blue
> The moon is %color
! set user.moonColor = $color
: OK: <color>

> What color is the moon
: The moon is <user.moonColor>
";

      Bot b1 = BuildBot(cfg);
      Bot b2 = BuildBot(cfg);

      string r1a = Invoke(b1, new Request("session1", "user1") { Input = "The moon is blue" });
      Assert.AreEqual("OK: blue", r1a);

      string r1b = Invoke(b2, new Request("session1", "user2") { Input = "The moon is red" });
      Assert.AreEqual("OK: red", r1b);

      string r2a = Invoke(b1, new Request("session1", "user1") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is blue", r2a);

      string r2b = Invoke(b2, new Request("session1", "user2") { Input = "What color is the moon?" });
      Assert.AreEqual("The moon is red", r2b);
    }
  }
}
