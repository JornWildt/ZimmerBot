using NUnit.Framework;

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
  > is it raining in {loc:location}
  > how is the weather in {loc:location} today
}

>> { intent = current_weather, type = question }
: The weather is good

>> { intent = current_weather, type = question, loc = * }
: The weather is good in '<loc>'

>> { intent = current_weather, type = question, loc = ""Smørum Ovre"" }
: The weather is always fantastic in Smørum Ovre
";
      BuildBot(cfg);

      AssertDialog("is it snowing", "The weather is good");
      AssertDialog("is it snowing today", "The weather is good");
      AssertDialog("I wonder if it is snowing", "The weather is good");

      AssertDialog("how hot is it", "The weather is good");
      //AssertDialog("how raining is weather", "The weather is good");

      AssertDialog("is it raining in new york", "The weather is good in 'new york'");
      AssertDialog("how raining is it in new york", "The weather is good in 'new york'");

      AssertDialog("how is the weather in Smørum Ovre today", "The weather is always fantastic in Smørum Ovre");

      AssertDialog("is green your favorite color?", "???", "Make sure completely unknown inputs does not match");
      AssertDialog("grumph idugh yuckmay", "???", "Make sure completely unknown inputs does not match");
    }
  }
}
