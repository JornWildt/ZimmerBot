using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class AnswerTests : TestHelper
  {
    [Test]
    public void CanParseSingleAnswer()
    {
      Domain d = ParseDomain(@"
> aa bb
! answer
{
  > yes
  : cool

  > no
  : okay, sorry
}
");

      Assert.AreEqual(1, d.Rules.Count);
      Assert.AreEqual(1, d.Rules[0].OutputStatements.Count);
      Assert.IsInstanceOf<AnswerOutputStatement>(d.Rules[0].OutputStatements[0]);
      AnswerOutputStatement os = (AnswerOutputStatement)d.Rules[0].OutputStatements[0];
      Assert.AreEqual(2, os.RuleGenerators.Count);
    }
  }
}
