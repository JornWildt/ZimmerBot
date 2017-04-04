#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class AbbeviationTests : TestHelper
  {
    [Test]
    public void CanRecognizeAbbrevations()
    {
      BuildBot(@"
! concept emo_surprise = WTF, W.T.F., W.T.F

> %emo_surprise
: What the fuck?");

      AssertDialog("WTF", "What the fuck");
    }
  }
}
#endif