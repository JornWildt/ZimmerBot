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
  ""Smørum Nedre""
}

! pattern (intent = current_weather, type = question)
{
  > is it snowing
  > how hot is it
  > is it raining in {entity:location:}
  > how is the weather in {entity:location} today
}

>> { intent: current_weather }
: The weather is good!

>> { intent: current_weather, p1 }
: The weather in '<1>' is good!
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
    }
  }
}
