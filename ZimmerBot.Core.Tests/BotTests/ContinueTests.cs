using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ContinueTests : TestHelper
  {
    [Test]
    public void CanContinueWithNewRule()
    {
      BuildBot(@"
> Hello
: Hi
! continue

>
: What can I help you with?

> Yo
: Yaj!
! continue (""__HowIsItGoing"")

> __HowIsItGoing
: How is it going?
");
      AssertDialog("Hello", "Hi\nWhat can I help you with?");

      AssertDialog("Yo!", "Yaj!\nHow is it going?");
    }
  }
}
