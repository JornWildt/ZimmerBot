using NUnit.Framework;
using System;
using System.Threading;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class PipelineTests : TestHelper
  {
    [Test]
    public void CanAddToStartOfPipeline()
    {
      BuildBot(@"
! pipeline start
! set session.X = 10

> test
: X = <session.X>
");
      AssertDialog("test", "X = 10");
      AssertDialog("test", "X = 10");
    }


    [Test]
    public void CanCallProcessorFromPipeline()
    {
      var thisDay = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      BuildBot($@"
! pipeline start
! call DateTime.IsItWeekDay(""{thisDay}"")
! set session.X = answer
! call DateTime.IsItWeekDay(""Not-a-day"")
! set session.Y = answer

> test
: X = <session.X> | Y = <session.Y>
");
      AssertDialog("test", "X = true | Y = false");
      AssertDialog("test", "X = true | Y = false");
    }


    [Test]
    public void PipelineOutputIsIncluded()
    {
      // Don't know if it is smart to include output,
      // but maybe someday someone wants a specific string generated
      // on each request - and it makes it easy to see that the pipeline item
      // is executed.

      BuildBot($@"
! pipeline start
: PIP

> test
: POP
");
      AssertDialog("test", "PIP\nPOP");
    }


    [Test]
    public void PipelineRespectsCondition()
    {
      BuildBot($@"
! pipeline start
! when (1 = 2)
: PIP

> test
: POP
");
      AssertDialog("test", "POP");
    }


    [Test]
    public void ProcessorOutputIsAvailableInOtherRules()
    {
      BuildBot($@"
! pipeline start
! call DateTime.Details()
! set tmp.isWednesday = isWednesday
: XXX

> test
: isWednesday = <tmp.isWednesday>
");
      string isWednesday = DateTime.Now.DayOfWeek == DayOfWeek.Wednesday ? "true" : "false";

      AssertDialog("test", $"XXX\nisWednesday = {isWednesday}");
    }
  }
}
