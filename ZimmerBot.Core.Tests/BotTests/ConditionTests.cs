using NUnit.Framework;
using System;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ConditionTests : TestHelper
  {
    [Test]
    public void CanHandleCondition()
    {
      BuildBot(@"
> Hi
! when (user.pet)
: Hi, I remember you have a <user.pet>

> Hi
! when (!user.pet)
: Hi

> I have a (dog|cat)
! set user.pet = $1
: Nice

");

      AssertDialog("Hi", "Hi");
      AssertDialog("I have a dog", "Nice");
      AssertDialog("Hi", "Hi, I remember you have a dog");
      AssertDialog("I have a cat", "Nice");
      AssertDialog("Hi", "Hi, I remember you have a cat");
    }


    [Test]
    public void CanHandleAnd()
    {
      BuildBot(@"
> Hi
! when user.pet = ""dog"" OR user.pet = ""cat""
: You have a dog or a cat

> Hi
! when user.pet = ""fish""
: You have a fish

> I have a (dog|cat|fish)
! set user.pet = $1
: Ok

");

      AssertDialog("Hi", "???");
      AssertDialog("I have a dog", "Ok");
      AssertDialog("Hi", "You have a dog or a cat");
      AssertDialog("I have a cat", "Ok");
      AssertDialog("Hi", "You have a dog or a cat");
      AssertDialog("I have a fish", "Ok");
      AssertDialog("Hi", "You have a fish");
    }


    [Test]
    public void CanHandleConditionInEmptyTriggers()
    {
      BuildBot(@"
>
! when (user.pet)
: Hi, I remember you have a <user.pet>

>
! when (!user.pet)
: Hi

> I have a (dog|cat)
! set user.pet = $1
: Nice

");

      AssertDialog("Hi", "???");
      AssertDialog("", "Hi");
      AssertDialog("I have a dog", "Nice");
      AssertDialog("", "Hi, I remember you have a dog");
      AssertDialog("I have a cat", "Nice");
      AssertDialog("", "Hi, I remember you have a cat");
    }


    [Test]
    public void CanUseNonExistingValues()
    {
      BuildBot(@"
> A
! when !tmp.Unknown
: OK-A

> B
! when !session.Unknown
: OK-B

> C
! when !user.Unknown
: OK-C

> D
! when !bot.Unknown
: OK-D
");

      AssertDialog("A", "OK-A");
      AssertDialog("B", "OK-B");
      AssertDialog("C", "OK-C");
      AssertDialog("D", "OK-D");
    }


    [Test]
    public void CanUseAndOrNot()
    {
      BuildBot(@"
> START
! set user.A = 'X'
: GO

> A
! when user.A
: OK-1

> B
! when NOT user.B
: OK-2

");

      AssertDialog("START", "GO");
      AssertDialog("A", "OK-1");
      AssertDialog("B", "OK-2");
    }


    [Test]
    public void ItThrowsWhenAccessingUnknownRootIdentifier()
    {
      BuildBot(@"
> E
! when !Unknown.Unknown
: ERROR
");

      Assert.Throws<InvalidOperationException>(() => AssertDialog("E", ""));
    }
  }
}
