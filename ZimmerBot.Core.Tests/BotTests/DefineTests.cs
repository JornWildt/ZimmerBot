using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class DefineTests : TestHelper
  {
    [Test]
    public void CanDefineWords()
    {
      BuildBot(@"
! define
{
  ""walk"": verb (""walking"", ""walked"").
  ""run"": verb (running, ran).
  wasabi: noun ().
  ""walkie talkie"": noun ().
  ""walt disney"": noun ().
}

! pattern (intent = echo)
{
  > echo {a}
}

>> { intent = echo, a = * }
: Got '<a>'
");

      AssertDialog("echo walk", "Got 'walk'");
      AssertDialog("echo walking", "Got 'walking'");
      AssertDialog("echo walked", "Got 'walked'");
      AssertDialog("echo wasabi", "Got 'wasabi'");
      AssertDialog("echo walkie talkie", "Got 'walkie talkie'");
      AssertDialog("echo Walt Disney", "Got 'Walt Disney'");
    }
  }
}
