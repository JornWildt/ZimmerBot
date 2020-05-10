using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class IntentWildcardTests : TestHelper
  {
    [Test]
    public void CanUseWildcardInPattern()
    {
      string cfg = @"
! define (type)
{
  film:.
  photo:.
}

! pattern (intent = find)
{
  > find all {t} about <query>
  > find {t} written about <query>
}

>> find (t:type)
: Searching (<t>) about: <query>.
";
      BuildBot(cfg);

      AssertDialog("find all film about dogs and cats", "Searching (film) about: dogs and cats.");
    }
  }
}
