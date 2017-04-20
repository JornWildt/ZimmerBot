﻿using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class PatternTests : TestHelper
  {
    [Test]
    public void CanMatchPatterns()
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
      AssertDialog("how raining is weather", "???");

      AssertDialog("is it raining in new york", "The weather is good in 'new york'");
      AssertDialog("how raining is it in new york", "The weather is good in 'new york'");

      AssertDialog("how is the weather in Smørum Ovre today", "The weather is always fantastic in Smørum Ovre");

      AssertDialog("is green your favorite color?", "???", "Make sure completely unknown inputs does not match");
      AssertDialog("grumph idugh yuckmay", "???", "Make sure completely unknown inputs does not match");
    }


    [Test]
    public void CanMatchEntitiesWithoutClass()
    {
      string cfg = @"
! entities (location)
{
  ""London""
}

! entities (person)
{
  ""Mario Zimmer""
}

! pattern (intent = where_is, type = question)
{
  > where is {item}
}

>> { intent = where_is, type = question, item = *}
: You can find '<item>' at home.
";
      BuildBot(cfg);

      AssertDialog("where is london", "You can find 'london' at home.");
      AssertDialog("where is Mario Zimmer", "You can find 'Mario Zimmer' at home.");
    }


    [Test]
    public void DoesNotMatchUnintended()
    {
      string cfg = @"
! entities (person)
{
  ""Mario Zimmer""
}

! pattern (intent = where_is, type = question)
{
  > where is {item}
  > where can I find {item}
}

! pattern (intent = how_heavy_is, type = question)
{
  > how heavy is {item}
  > what does {item} weight
}

>> { intent = where_is, type = question, item = *}
: You can find '<item>' at home.

>> { intent = how_heavy_is, type = question, item = *}
: '<item>' is heavy.
";
      BuildBot(cfg);

      AssertDialog("where is Mario Zimmer", "You can find 'Mario Zimmer' at home.");
      AssertDialog("where heavy can Mario Zimmer be", "You can find 'Mario Zimmer' at home.");
      AssertDialog("Mario Zimmer ran away", "???");
      AssertDialog("what is an animal", "???");
    }


    [Test]
    public void PatternMatchIsCaseInsensitive()
    {
      string cfg = @"
! entities (person)
{
  ""Mario Zimmer""
}

! pattern (intent = where_is, type = question)
{
  > where is {item}
}

>> { intent = where_is, type = question, item = *}
: '<item>' is here.
";
      BuildBot(cfg);

      AssertDialog("where is Mario Zimmer", "'Mario Zimmer' is here.");
      AssertDialog("WHERE iS mario ZIMmer", "'mario ZIMmer' is here.");
    }
  }
}
