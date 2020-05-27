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
  > find all about <query> in {t}
  > find <query> in {t}
  > what {t} related to <query> are you able to find
}

>> find (t:type)
: Searching (<t>) about: <query>.
";
      BuildBot(cfg);

      AssertDialog("find all film about dogs and cats", "Searching (film) about: dogs and cats.");
      AssertDialog("find nonsense about the usual stuff", "???");
      AssertDialog("find all about cats and dogs in photo", "Searching (photo) about: cats and dogs.");
      AssertDialog("what photo related to white water rafting are you able to find", "Searching (photo) about: white water rafting.");
      AssertDialog("find dungeons and dragons in photo", "Searching (photo) about: dungeons and dragons.");

      // "stuff about" does not match any pattern, so it gets included in "> find <query> in {t}"
      // AssertDialog("find stuff about cats and dogs in photo", "Searching (photo) about: cats and dogs.");
    }
  }
}
