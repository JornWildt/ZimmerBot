using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class EventTests : TestHelper
  {
    [Test]
    public void CanReactToWelcomeEvent()
    {
      BuildBot(@"
! on (welcome)
{
  : Welcome
}

> Hi
: Hello
");

      Request request = new Request(Guid.NewGuid().ToString(), "default") { EventType = Request.EventEnum.Welcome  };
      string response = Invoke(B, request);
      Assert.AreEqual("Welcome", response);

      AssertDialog("hi", "Hello");
    }
  }
}
