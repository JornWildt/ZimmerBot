using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class TopicTests : TestHelper
  {
    [Test]
    public void CanUseTopic()
    {
      // Arrange
      BuildBot(@"

> help
: It is okay

> * dead *
: You say dead? Ever heard of zombies?
! start_topic Zombies

! topic Zombies
{
  T> Zombies are foul creatures of the dark
  T> Run!
  T> They are comming!

  > help
  : The zombiecalypse is comming! Run, you fools, run!
}
");

      // Act
      AssertDialog("Xxx", "???", "This does not match anything");
      AssertDialog("help", "It is okay", "No topic started yet - match default help");
      AssertDialog("Are you dead?", "You say dead? Ever heard of zombies?", "Start topic");
      AssertDialog("help", "The zombiecalypse is comming! Run, you fools, run!", "Topic started - match topic help");
      AssertDialog("See, a zombie!", "Zombies are foul creatures of the dark", "First line in topic");
      AssertDialog("now what", "Run!", "Second line in topic");
      AssertDialog("help", "The zombiecalypse is comming! Run, you fools, run!", "Topic started - match topic help");
      AssertDialog("Lovely", "They are comming!", "Last line in topic");
      AssertDialog("help", "It is okay", "Topic is now empty - use default help again");
    }


    [Test]
    public void CanSelectRuleFromInactiveTopicAndActivateIt()
    {
      // Arrange
      BuildBot(@"

> help
: It is okay

! topic Zombies
{
  T> Zombies are foul creatures of the dark
  T> Run!
  T> They are comming!

  > where is the zombie
  : In the darkness

  > help
  : The zombiecalypse is comming! Run, you fools, run!
}
");

      // Act
      AssertDialog("Xxx", "???", "This does not match anything");
      AssertDialog("help", "It is okay", "No topic started yet - match default help");
      AssertDialog("Where is the zombie?", "In the darkness", "Start topic");
      AssertDialog("help", "The zombiecalypse is comming! Run, you fools, run!", "Topic started - match topic help");
      AssertDialog("See, a zombie!", "Zombies are foul creatures of the dark", "First line in topic");
    }


    [Test]
    public void CanHandleAnswer()
    {
      // Arrange
      BuildBot(@"
! topic Zombies
{
  T> Zombies contaminates
  ! answer
  {
    > how
    : through a virus
  }

  T> Zombies are slow
  ! answer
  {
    > why
    : they are dead
      +: and rotten!
  }

  > where is the zombie
  : In the darkness
}
");

      // Act
      AssertDialog("Where is the zombie?", "In the darkness", "Start topic");
      AssertDialog("Duh", "Zombies contaminates", "Expect answer");
      AssertDialog("how", "through a virus");
      AssertDialog("Duh", "Zombies are slow");
      AssertDialog("why", "they are dead (...)");
    }


    [Test]
    public void CanMarkRulesAsInactiveWithoutTopic()
    {
      // Arrange
      BuildBot(@"

> now what
! weight 0.5
: Relax

! topic Zombies
{
  T> Zombies are foul creatures of the dark

  > where is the zombie
  : In the darkness

  > now what
  : Run!
}
");

      // Act
      AssertDialog("now what", "Relax");
      AssertDialog("now what", "Relax");
      AssertDialog("where is the zombie", "In the darkness");
      AssertDialog("now what", "Run!");
    }


    [Test]
    public void CanHandleAnswersInTopicRules()
    {
      BuildBot(@"
! topic Zombies
{
  > Zombie
  : Run!

  > xxx
  : Agree?
  ! answer
  {
    > yes
    : Good

    > no
    : Sorry
  }
}
");

      AssertDialog("yes", "???", "Not expected at this time");
      AssertDialog("zombie", "Run!", "Start topic");
      AssertDialog("xxx", "Agree?", "Register expected answers");
      AssertDialog("yes", "Good", "Should match expected 'yes' answer");
    }


    [Test]
    public void ItDoesNotActivateDefaultTopicAutomatically()
    {
      BuildBot(@"
> why
: Because

> xxx
: Not a zombie

> yyy
: Neither a zombie

! topic Zombies
{
  > Zombie
  : Run!

  > yyy
  : Still zombies
}
");
      AssertDialog("Zombie", "Run!", "Start topic");
      AssertDialog("xxx", "Not a zombie", "Match default topic - but do not make it current");
      AssertDialog("yyy", "Still zombies", "Match in topic");
    }


    [Test]
    [Ignore("Not ready yet")]
    public void CanSwitchBetweenTopics()
    {
      // Arrange
      BuildBot(@"
! topic Zombies (zombie, dead, rotten)
{
  > now what
  : Run, you fools, run!

  > *
  ! weight 0.5
  : The zombiecalypse is comming!
}

! topic Computers (pc, ipad, software)
{
  > now what
  : Restart

  > *
  ! weight 0.5
  : Pull the power and try again.
}
");

      // Act
      AssertDialog("Xxx", "???");
      AssertDialog("now what", "???");
      AssertDialog("See, a zombie!", "The zombiecalypse is comming!");
      AssertDialog("now what", "Run, you fools, run!");
      AssertDialog("help me with my pc", "Pull the power and try again.");
      AssertDialog("now what", "Restart");
    }
  }
}
