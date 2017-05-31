using NUnit.Framework;

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
: Hi, I remeber you have a <user.pet>

> Hi
! when (!user.pet)
: Hi

> I have a (dog|cat)
! set user.pet = $1
: Nice

");

      AssertDialog("Hi", "Hi");
      AssertDialog("I have a dog", "Nice");
      AssertDialog("Hi", "Hi, I remeber you have a dog");
      AssertDialog("I have a cat", "Nice");
      AssertDialog("Hi", "Hi, I remeber you have a cat");
    }


    [Test]
    public void CanHandleConditionInEmptyTriggers()
    {
      BuildBot(@"
>
! when (user.pet)
: Hi, I remeber you have a <user.pet>

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
      AssertDialog("", "Hi, I remeber you have a dog");
      AssertDialog("I have a cat", "Nice");
      AssertDialog("", "Hi, I remeber you have a cat");
    }
  }
}
