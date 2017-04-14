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
