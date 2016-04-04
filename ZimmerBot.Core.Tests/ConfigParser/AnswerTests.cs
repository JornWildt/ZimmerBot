using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
#if false
  [TestFixture]
  public class AnswerTests : TestHelper
  {
    [Test]
    public void CanParseSingleAnswer()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
> aa bb
! answer
{
  > yes
  : cool

  > no
  : okay, sorry
}
");

      Assert.AreEqual(1, kb.Rules.Count);
      Assert.AreEqual(1, kb.Rules[0].OutputStatements.Count);
      Assert.IsInstanceOf<AnswerOutputStatement>(kb.Rules[0].OutputStatements[0]);
      AnswerOutputStatement os = (AnswerOutputStatement)kb.Rules[0].OutputStatements[0];
      Assert.AreEqual(2, os.Rules.Count);
    }
  }
#endif
}
