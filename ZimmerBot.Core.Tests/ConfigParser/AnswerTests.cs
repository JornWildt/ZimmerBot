using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class AnswerTests : TestHelper
  {
    [Test]
    public void CanParseSingleAnswer()
    {
      Rule r = ParseRule(@"
> aa bb
! answer
{
  > yes
  : cool

  > no
  : okay, sorry
}
");

    }
  }
}
