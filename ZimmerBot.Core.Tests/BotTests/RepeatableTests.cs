using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class RepeatableTests : TestHelper
  {
    [Test]
    public void NormalyOutputDoesNotRepeat()
    {
      BuildBot(@"
> help
: It is okay

> *
! weight 0.999
: It is done
");
      AssertDialog("help", "It is okay");
      AssertDialog("help", "It is done");
    }


    [Test]
    public void CanMarkRuleAsRepeatable()
    {
      BuildBot(@"
> help
: It is okay
! repeatable
");

      AssertDialog("help", "It is okay");
      AssertDialog("help", "It is okay");
    }


    [Test]
    public void CallStatementsMakeRuleRepeatle()
    {
      BuildBot(@"
> help
: It is okay
! call General.Echo(""aaa"")
");

      AssertDialog("help", "It is okay");
      AssertDialog("help", "It is okay");
    }


    [Test]
    public void CanMarkAsNonRepeatable()
    {
      BuildBot(@"
> help
: It is okay
! not_repeatable
! call General.Echo(""aaa"")

> *
! weight 0.999
: It is done
");

      AssertDialog("help", "It is okay");
      AssertDialog("help", "It is done");
    }
  }
}
