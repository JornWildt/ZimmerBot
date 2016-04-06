﻿using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;

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
      AssertDialog("no", "okay, sorry");
    }
  }
}