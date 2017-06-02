using NUnit.Framework;

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

>> { intent = current_weather, type = question, loc = ""Smørum Nedre"" }
: The weather is always fantastic in Smørum Nedre
";
      BuildBot(cfg);

      AssertDialog("is it snowing", "The weather is good");
      AssertDialog("is it snowing today", "The weather is good");
      AssertDialog("I wonder if it is snowing", "The weather is good");

      AssertDialog("how hot is it", "The weather is good");
      //AssertDialog("how raining is weather", "???");

      AssertDialog("is it raining in new york", "The weather is good in 'new york'");
      AssertDialog("how raining is it in new york", "The weather is good in 'new york'");

      AssertDialog("how is the weather in Smørum Nedre today", "The weather is always fantastic in Smørum Nedre");

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


    [Test]
    public void CanMatchConcepts()
    {
      string cfg = @"
! concept kill = kill, murder
! pattern ( intent = how_to_kill )
{
  > how do you kill {something}
}

>> { intent = how_to_kill }
: With an axe
";

      BuildBot(cfg);

      AssertDialog("how do you kill a zombie", "With an axe");
    }


    [Test]
    public void CanMatchRecursiveConcepts()
    {
      string cfg = @"
! concept kill_1 = murder, stab
! concept kill = kill, %kill_1

! pattern ( intent = how_to_kill )
{
  > how do you kill {something}
}

>> { intent = how_to_kill }
: With an axe
";

      BuildBot(cfg);

      AssertDialog("how do you kill a zombie", "With an axe");
      AssertDialog("how do you murder a zombie", "With an axe");
    }


    [Test]
    public void ItDoesNotSelectAnswerWithoutQuestion()
    {
      BuildBot(@"
! pattern (intent = yes)
{
  > yes
  > aye
  > sure
}

> Zombie
: Run!
! start_topic Zombies

! topic Zombies
{
  > xxx
  : Agree?
  ! answer
  {
    >> { intent = yes }
    : Good

    >> { intent = no }
    : Sorry
  }
}
");

      AssertDialog("zombie", "Run!", "Start topic");
      AssertDialog("yes", "???", "Should NOT match (un)expected 'yes' answer");
    }


    [Test]
    public void CanUseWeightInPatterns()
    {
      BuildBot(@"
! pattern (intent = what, type = question )
{
  > what
}

>> { intent = what }
: That!

>> { type = question }
! weight 0.5
: Yes!
: Aha!
: Yep!
: No!
");

      AssertDialog("what", "That!");
    }


    [Test]
    public void ParameterNameIsCaseInsensitive()
    {
      BuildBot(@"
! entities (names)
{
  > lisa
}

! pattern (intent = my_name)
{
  > my name is {name}
}

>> my_name (name = LISA)
: Welcome <name>
");
      AssertDialog("my name is Lisa", "Welcome Lisa");
    }


    [Test]
    public void CanMatchIntentShortHand()
    {
      BuildBot(@"
! pattern (intent = what)
{
  > what
  > why that
  > how come
}

>> what
: That!
");
      AssertDialog("Hello", "???");
      AssertDialog("why that", "That!");
      AssertDialog("what", "That!");
    }


    [Test]
    public void CanMatchIntentAndParameterShortHand()
    {
      BuildBot(@"
! entities (names)
{
  > john
  > peter
  > lisa
}

! pattern (intent = my_name)
{
  > my name is {name}
  > you can call me {name}
}

# This is equivalent to >> { intent = my_name, name = * }
>> my_name (name)
: Hi, <name>

# This is equivalent to >> { intent = my_name, name = lisa }
>> my_name (name = Lisa)
: Welcome <name>
");
      AssertDialog("my name is john", "Hi, john");
      AssertDialog("you can call me Peter", "Hi, Peter");
      AssertDialog("my name is Lisa", "Welcome Lisa");
    }


    [Test]
    public void CanMatchMultiplePatterns()
    {
      BuildBot(@"
! pattern (intent = what)
{
  > what
}

! pattern (intent = why)
{
  > why that
}

>> what
>> why
: That!
");
      AssertDialog("Hello", "???");
      AssertDialog("why that", "That!");
      AssertDialog("what", "That!");
    }


    [Test]
    public void CanMatchStopWord()
    {
      BuildBot(@"
! pattern (intent = my_name)
{
  > my name is ~not
}

! pattern (intent = your_country)
{
  > your country is
}

>> my_name
: NAME

>> your_country
: COUNTRY
");

      AssertDialog("my name is", "NAME");
      AssertDialog("your country is", "COUNTRY");
      AssertDialog("my name is not", "???");
    }


    [Test]
    public void CanIncludeClassInParameterMatch()
    {
      BuildBot(@"
! define (animal)
{
  elephant:.
  bear:.
}

! define (place)
{
  copenhagen:.
  stockholm:.
}

! pattern (intent = where_is)
{
  > where is {x}
}

>> { intent = where_is, x:animal }
: The <x> is located in zoo.

>> where_is (x:place)
: <x> is in Scandinavia
");

      AssertDialog("where is the elephant", "The elephant is located in zoo.");
      AssertDialog("where is the bear", "The bear is located in zoo.");
      AssertDialog("where is copenhagen", "copenhagen is in Scandinavia");
      AssertDialog("where is Stockholm", "Stockholm is in Scandinavia");
    }


    [Test]
    public void SamePatternCanMatchMultipleClasses()
    {
      BuildBot(@"
! define (place)
{
  Oslo:.
}

! define (animal)
{
  horse:.
}

! pattern (intent = where_is)
{
  > where is {x}
}

>> where_is (x:place)
: In Norway

>> where_is (x:animal)
: In the field
");

      AssertDialog("why not", "???");
      AssertDialog("where is oslo", "In Norway");
      AssertDialog("where is the horse", "In the field");
    }


    [Test]
    public void ItChoosesExactMatchOverSubMatch()
    {
      BuildBot(@"
! pattern (intent = abc)
{
  > a b c
}

! pattern (intent = ab_c)
{
  > a b {c}
}

! pattern (intent = ab_cd)
{
  > a b {c} {d}
}

! pattern (intent = ab)
{
  > a b
}

! pattern (intent = c)
{
  > {c}
}

! define (word)
{
  ccc:.
  ddd:.
}

>> ab
! weight 0.99
: Got AB

>> abc
: Got ABC

>> ab_c
: Got AB_C

>> ab_cd
: Got AB_CD

>> c
: Got CCC
");

      AssertDialog("a b", "Got AB");
      AssertDialog("a b c", "Got ABC");
      AssertDialog("a b ccc", "Got AB_C");
      AssertDialog("a b ccc ddd", "Got AB_CD");
      AssertDialog("x", "???");
      AssertDialog("ccc", "Got CCC");
    }


    [Test]
    public void ItChecksTypes()
    {
      BuildBot(@"
! define (city)
{
  Oslo:.
  Stockholm:.

  # Silly city name (but real in danish).
  # It provokes having two city names in ""where is oslo"" and thus matching is_x_in_y.
  # It is then handled correctly because the evaluation algorithm takes the word ordering into account.
  Where:.
}

! pattern (intent = where_is)
{
  > where is {x}
}

! pattern (intent = is_x_in_y)
{
  > is {x} in {y}
}

>> where_is (x:city)
: Here

>> is_x_in_y (x:city, y:city)
: No
");

      AssertDialog("where is oslo", "Here");
      AssertDialog("is oslo in stockholm", "No");
    }
  }
}
