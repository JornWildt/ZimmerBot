using NUnit.Framework;
using System.Linq;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class SchedulerParserTests : TestHelper
  {
    [Test]
    public void CanParseScheduledAction()
    {
      string s = @"
! every ""1 2 3 4 5""
: HI
";

      KnowledgeBase kb = ParseKnowledgeBase(s);

      Assert.AreEqual(1, kb.ScheduledActions.Count);
      var action = kb.ScheduledActions.First().Value;
      Assert.AreEqual("1 2 3 4 5", action.CronExpr);
      Assert.AreEqual(1, action.Statements.Count);
      Assert.AreEqual(0, action.Modifiers.Count);
    }
  }
}
