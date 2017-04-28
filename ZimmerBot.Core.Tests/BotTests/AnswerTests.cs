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


    [Test]
    public void CanTakeAnswerFromOtherRule()
    {
      BuildBot(@"
> How are you
: Fine. And you?
! answer at how_are_you

<how_are_you>
: How are you?
! answer
{
  > fine
  : Cool

  > Dreadful
  : Sorry to hear that
}
");

      AssertDialog("How are you", "Fine. And you?");
      AssertDialog("Fine", "Cool");

      AssertDialog("How are you", "Fine. And you?");
      AssertDialog("Dreadful", "Sorry to hear that");
    }


    [Test]
    public void CannotUseAnswersWithoutPrecedingQuestion()
    {
      BuildBot(@"
> How are you
: Fine. And you?
! answer
{
  > fine
  : Cool

  > Dreadful
  : Sorry to hear that
}
");

      AssertDialog("Fine", "???");
      AssertDialog("Dreadful", "???");
    }


    [Test]
    public void CannotUseAnswersWithoutPrecedingQuestion2()
    {
      BuildBot(@"
> help
: Just a second

> How are you
: Fine. And you?
! answer
{
  > fine
  : Cool

  > Dreadful
  : Sorry to hear that
}
");

      AssertDialog("How are you", "Fine. And you?");
      AssertDialog("help", "Just a second");
      AssertDialog("Fine", "???");
      AssertDialog("Dreadful", "???");
    }


    [Test]
    public void PatternRulesAndRegexRulesAreTriggeredEqualyInAnswers()
    {
      // Arrange
      BuildBot(@"
! pattern (intent = yes)
{
  > yes
}

> What
: Yes or No?
! answer
{
  >> { intent = yes }
  : Great!

  > +
  : Okay
}

");

      // Act
      AssertDialog("What", "Yes or No?");
      AssertDialog("Yes", "Great!");
    }
  }
}
