using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class KeepStateTests : TestHelper
  {
    [Test]
    public void ItRemembersStateForReferencingStatements()
    {
      BuildBot(@"
! define (car)
{
  Volvo:.
  Ford:.
  Toyota:.
}

! pattern (intent = do_you_like_x)
{
  > do you like {topic}
  > do you like it
}

! pattern (intent = do_you_have_x)
{
  > do you have {topic}
  > do you have one
}

! pattern (type = spm)
{
  > what about {topic}
}

>> do_you_like_x (topic:car)
: Yes, <topic> is a great car!

>> do_you_have_x (topic:car)
: No, I do not own <topic>
");
      AssertDialog("do you like the Volvo", "Yes, Volvo is a great car!");
      AssertDialog("what about a Ford", "Yes, Ford is a great car!");
      AssertDialog("do you have one", "No, I do not own Ford");
      AssertDialog("do you have a Toyota", "No, I do not own Toyota");
      AssertDialog("do you have one", "No, I do not own Toyota");
      AssertDialog("do you like it", "Yes, Toyota is a great car!");
    }
  }
}
