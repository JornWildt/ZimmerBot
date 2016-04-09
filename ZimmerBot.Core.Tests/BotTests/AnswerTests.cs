using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class AnswerTests : TestHelper
  {
    [Test]
    public void CanAnswer()
    {
      BuildBot(@"
> yes
: why

> no
: why not

> let us play
! answer
{
  > yes
  : cool

  > no
  : okay, sorry
}
: are you ready?
");

      AssertDialog("yes", "why");
      AssertDialog("no", "why not");
      AssertDialog("let us play", "are you ready?");
      AssertDialog("yes", "cool");
      AssertDialog("let us play", "are you ready?");
      AssertDialog("no", "okay, sorry");
    }


    [Test]
    public void CanAnswerContinuedQuestion()
    {
      BuildBot(@"
> hi
: welcome
! continue

>
: how are you doing?
! answer
{
  > fine
  : good!
}

> fine
: okay
");

      AssertDialog("fine", "okay");
      AssertDialog("hi", "welcome\nhow are you doing?");
      AssertDialog("fine", "good!");
    }
  }
}
