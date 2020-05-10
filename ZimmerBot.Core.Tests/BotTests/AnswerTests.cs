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

> *
! weight 0.5
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
    public void CanUseAnswerWithPatternStarter()
    {
      // Arrange
      BuildBot(@"
! pattern (intent = start)
{
  > start
}

>> start
! answer
{
  > green
  : Green!

  > red
  : Red!
}
: Start - green or red?

> *
: Default

");
      // Act+Assert
      AssertDialog("X", "Default");
      AssertDialog("green", "Default");
      AssertDialog("red", "Default");
      AssertDialog("start", "Start - green or red?");
      AssertDialog("red", "Red!");
      AssertDialog("green", "Default");
      AssertDialog("red", "Default");
      AssertDialog("start", "Start - green or red?");
      AssertDialog("green", "Green!");
    }




    [Test]
    public void CanUseAnswerWithPatternAndEntityStarter()
    {
      // Arrange
      BuildBot(@"
! define (T)
{
  O:.
}

! pattern (intent = start)
{
  > start {x}
  > start
}

>> start
! continue with start O

>> start (x:*)
! answer
{
  > green
  : Green!

  > red
  : Red!
}
: Start - green or red?

> *
: Default

");
      // Act+Assert
      AssertDialog("X", "Default");
      AssertDialog("green", "Default");
      AssertDialog("red", "Default");
      AssertDialog("start O", "Start - green or red?");
      AssertDialog("red", "Red!");
      AssertDialog("green", "Default");
      AssertDialog("red", "Default");
      AssertDialog("start", "Start - green or red?");
      AssertDialog("green", "Green!");
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
  # Add tiny weight to overrule random choice
  ! weight 1.01
  : Great!

  > +
  : Okay
}

");

      // Act
      AssertDialog("What", "Yes or No?");
      AssertDialog("Yes", "Great!");
    }


    [Test]
    public void CanHavePatternTrees()
    {
      // Arrange
      BuildBot(@"
> A
: Aaaa
! answer
{
  > B
  : Bbbb

  > C
  : Cccc
  ! answer
  {
    > Q
    : Qqqq

    > W + Z
    : WXZ
  }
}");
      AssertDialog("A", "Aaaa");
      AssertDialog("C", "Cccc");
      AssertDialog("Q", "Qqqq");

      AssertDialog("A", "Aaaa");
      AssertDialog("C", "Cccc");
      AssertDialog("W x Z", "WXZ");
    }
  }
}
