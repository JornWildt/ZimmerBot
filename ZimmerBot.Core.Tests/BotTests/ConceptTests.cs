using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class ConceptTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      BuildBot(@"
! concept fruit = bananas, apple, strawberry
! concept meat = pork, chicken, beef
! concept fruit_and_meat = %meat, %fruit
! concept like = like, enjoy, prefer, really like

> I %like %fruit_and_meat
: I <like> <fruit_and_meat> too

> You %like %fruit_and_meat
! set session.like = $fruit_and_meat
: Okay, I <like> <fruit_and_meat> too

> What do you like
: I like <session.like>
");
    }


    [TestCase("I like bananas", "I like bananas too")]
    [TestCase("I enjoy bananas", "I enjoy bananas too")]
    [TestCase("I prefer apple", "I prefer apple too")]
    [TestCase("I like chicken", "I like chicken too")]
    [TestCase("I really like chicken", "I really like chicken too")]
    [TestCase("I LIKE chiCKEN", "I LIKE chiCKEN too")] // FIXME: improve casing on match ...
    public void CanMatchConcepts(string input, string expectedAnswer)
    {
      string output = Invoke(input);
      Assert.AreEqual(expectedAnswer, output);
    }


    [Test]
    public void CanReferenceConceptsInExpression()
    {
      string r1 = Invoke("You prefer beef");
      Assert.AreEqual("Okay, I prefer beef too", r1);

      string r2 = Invoke("What do you like?");
      Assert.AreEqual("I like beef", r2);
    }


    [Test]
    public void CanHandleStrings()
    {
      BuildBot(@"
! concept names = Hans, ""Hans-Christian""

> hi (%names)
: Hello '<1>'
");

      AssertDialog("Hi Hans", "Hello 'Hans'");
      AssertDialog("Hi \"Hans-Christian\"", "Hello 'Hans-Christian'");
    }
  }
}
