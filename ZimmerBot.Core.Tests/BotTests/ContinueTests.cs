using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ContinueTests : TestHelper
  {
    [Test]
    public void CanContinueWithEmptyTarget()
    {
      BuildBot(@"
> Hello
: Hi
! continue

> *
! weight 0.5
: What can I help you with?
");
      AssertDialog("Hello", "Hi\nWhat can I help you with?");
    }


    [Test]
    public void CanContinueWithLabel()
    {
      BuildBot(@"
> Yo
: Yaj!
! continue at HowIsItGoing

<HowIsItGoing>
: How is it going?

>
: What can I help you with?
");
      AssertDialog("Yo!", "Yaj!\nHow is it going?");
    }


    [Test]
    public void CanContinueWithNewInput()
    {
      BuildBot(@"
> Yo
: Yaj!
! continue with Having Fun

> Having Fun
: is it fun

>
: What can I help you with?
");
      AssertDialog("Yo!", "Yaj!\nis it fun");
    }


    [Test]
    public void CanContinueWithParameters()
    {
      BuildBot(@"
> Yo +
: Yaj!
! continue with Some <1>

> Some +
: Love '<1>'
");
      AssertDialog("Yo Mouse", "Yaj!\nLove 'Mouse'");
    }
  }
}
