﻿using NUnit.Framework;

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
> where is the zombie
: In the darkness
! start_topic Zombies

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
> where is the zombie
: In the darkness
! start_topic Zombies

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

  T> this is here to have an extra topic story line

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
    public void CanHandleAnswersInTopicRules()
    {
      BuildBot(@"
> Zombie
: Run!
! start_topic Zombies

! topic Zombies
{
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
    public void ItDoesNotSelectAnswerWithoutQuestion()
    {
      BuildBot(@"
> Zombie
: Run!
! start_topic Zombies

! topic Zombies
{
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

      AssertDialog("zombie", "Run!", "Start topic");
      AssertDialog("yes", "???", "Should NOT match (un)expected 'yes' answer");
      AssertDialog("no", "???", "Should NOT match (un)expected 'no' answer");
    }


    [Test]
    public void ItDoesNotActivateDefaultTopicAutomaticallyWhenHavingTopicStory()
    {
      BuildBot(@"
> Zombie
: Run!
! start_topic Zombies

> why
: Because

> xxx
: Not a zombie

> yyy
: Neither a zombie

! topic Zombies
{
  > yyy
  : Still zombies

  T> Run!
}
");
      AssertDialog("Zombie", "Run!", "Start topic");
      AssertDialog("xxx", "Not a zombie", "Match default topic - but do not make it current since there is a story to be told");
      AssertDialog("yyy", "Still zombies", "Match in topic");
    }


    [Test]
    public void CanMarkTopicRuleAsNotRepeating()
    {
      BuildBot(@"
> xxx
: Wow zombie
! not_repeatable
! start_topic Zombies

> xxx
! weight 0.999
: No more

! topic Zombies
{
  T> Zombie 1

  T> Zombie 2
}
");
      AssertDialog("xxx", "Wow zombie", "Start topic");
      AssertDialog("kkkk", "Zombie 1", "Is in topic");
      AssertDialog("xxx", "Zombie 2", "Not starting topic again");
      AssertDialog("xxx", "No more", "Topic empty, no exact match anymore");
    }


    [Test]
    public void ItDoesNotCountTopicRulesAsUsedWhenSelectingOtherRules()
    {
      // Arrange
      BuildBot(@"
> Help
: Zombies!
! start_topic Zombies

> what is that
: That

! topic Zombies
{
  T> Text1
  T> Text2
  T> Text3
  T> Text4
}
");

      // Act
      AssertDialog("What is that", "That");
      AssertDialog("help", "Zombies!");
      AssertDialog("X", "Text1");
      AssertDialog("What is that", "That");
      AssertDialog("What is that", "That");
      AssertDialog("X", "Text2");
      AssertDialog("X", "Text3");
    }


    [Test]
    public void CanIncludeTopicStarters()
    {
      BuildBot(@"
! topic Weather
[
  > what a weather
  : Is it raining?
]
{
  T> I love rain
}
");

      AssertDialog("Xxx", "???");
      AssertDialog("what a weather", "Is it raining?");
      AssertDialog("Xxx", "I love rain");
    }


    [Test]
    public void TopicStartersDoNotTriggerWhenTopicIsActive()
    {
      BuildBot(@"
> Go
: Going

! topic fun
[
  > Start
  : Starting

  > Stop
  : Stopping
]
{
  T> Fun
  T> More fun
}
");
      AssertDialog("Xxx", "???");
      AssertDialog("Go", "Going");
      AssertDialog("Start", "Starting");
      AssertDialog("Stop", "Fun");
      AssertDialog("Start", "More fun");
    }


    [Test]
    public void CanAnswerTopicStarter()
    {
      BuildBot(@"
! topic religion
[
  > now what
  : do you belive in the great Umpf?
  ! answer
  {
    > yes
    : Good

    > no
    : Botchya!
  }
]
{
  T> The Umpf is great
  T> Umpf is almighty
}
");

      AssertDialog("Yes", "???");
      AssertDialog("now what", "do you belive in the great Umpf?");
      AssertDialog("Yes", "Good");
      AssertDialog("Yes", "The Umpf is great");
    }


#if false
    // The problem here is that "topic is empty" is not possible to figure out! The topic might be relevant even if it has no T> rules ...

    [Test]
    public void TopicStartersDoNotTriggerWhenTopicIsEmpty()
    {
      BuildBot(@"
> Go
: Going

! topic fun
[
  > Start
  : Starting
]
{
  T> Fun
  T> More fun
}
");
      AssertDialog("Xxx", "???");
      AssertDialog("Go", "Going");
      AssertDialog("Start", "Starting");
      AssertDialog("Xxx", "Fun");
      AssertDialog("Xxx", "More fun");
      AssertDialog("Xxx", "???");
      AssertDialog("Start", "???");
    }
#endif
  }
}
