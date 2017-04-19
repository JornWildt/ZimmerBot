using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class PatternTests : TestHelper
  {
    [Test]
    public void CanParseWeatherRequests()
    {
      string cfg = @"
! entities (location)
{
  ""London"",
  ""New York"",
  ""Smørum Nedre"",
  ""Tølløse Ovre""
}

! pattern (intent = current_weather, type = question)
{
  > is it snowing
  > how hot is it
  > is it raining in {l:location}
  > how is the weather in {l:location} today
}

>> { intent = current_weather, type = question }
: The weather is good

>> { intent = current_weather, type = question, l = * }
: The weather is good in '<l>'

>> { intent = current_weather, type = question, l = ""Smørum Ovre"" }
: The weather is always fantastic in Smørum Ovre
";
      BuildBot(cfg);

      //AssertDialog("is it snowing", "The weather is good");
      //AssertDialog("is it raining in new york", "The weather is good in 'new york'");
      AssertDialog("how is the weather in Smørum Ovre today", "The weather is always fantastic in Smørum Ovre");
    }
  }
}
