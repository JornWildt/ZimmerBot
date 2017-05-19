using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class EntityParserTests : TestHelper
  {
    [Test]
    public void CanAddEntities()
    {
      // Arrange
      string cfg = @"
! entities (city)
{
  ""Århus"",
  ""London"",
  ""Søndre Omme"",
  ""Nørre Omme""
}

! entities (person)
{
  ""Lise Nielsen"",
  ""Lars Malmø"",
  ""Peter Malmø"",
  ""Lars Bo Petersen""
}
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);

      Assert.AreEqual(2, kb.EntityManager.EntityClasses.Count);
      Assert.IsTrue(kb.EntityManager.EntityClasses.ContainsKey("city"));
      Assert.IsTrue(kb.EntityManager.EntityClasses.ContainsKey("person"));

      // total word count = twc = 15
      // P(city,0) = count(city,0) / twc = 4 / 15 = 0.267
      // P(city,1) = count(city,1) / twc = 2 / 15 = 0.133
      // P(århus,0|city,0) = count(århus,0) / count(city,0) = 1 / 4 = 0.250
      // P(city,0|århus,0) = P(city,0) * P(århus,0|city,0) = 0.267 * 0.250 = 0.067
      // P(omme,1|city,1) = count(omme,1) / count(city,1) = 2 / 2 = 1
      // P(city,1|omme,1) = P(city,1) * P(omme,1|city,1) = 0.133 * 1 = 0.133

      EntityClass city = kb.EntityManager.EntityClasses["city"];
      Assert.AreEqual(2, city.LongestWordCount);
      Assert.AreEqual(6, city.TotalNumberOfWords);
      Assert.AreEqual(0.0, city.ProbabilityFor("Berlin", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Lise", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Omme", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Århus", 1));
      Assert.That<double>(city.ProbabilityFor("Århus", 0), Is.EqualTo(0.067).Within(0.001));
      Assert.That<double>(city.ProbabilityFor("Omme", 1), Is.EqualTo(0.133).Within(0.001));

      // P(person,0) = count(person,0) / twc = 4 / 15 = 0.267
      // P(person,1) = count(person,1) / twc = 4 / 15 = 0.267
      // P(person,2) = count(person,2) / twc = 1 / 15 = 0.067
      // P(lars,0|person,0) = count(lars,0) / count(person,0) = 2 / 4 = 0.5
      // P(person,0|lars,0) = P(person,0) * P(lars,0|person,0) = 0.267 * 0.5 = 0.134
      // P(petersen,2|person,2) = count(petersen,2) / count(person,2) = 1 / 1 = 1
      // P(person,2|petersen,2) = P(person,2) * P(petersen,2|person,2) = 0.067 * 1 = 0.067
      EntityClass person = kb.EntityManager.EntityClasses["person"];
      Assert.AreEqual(3, person.LongestWordCount);
      Assert.AreEqual(9, person.TotalNumberOfWords);
      Assert.AreEqual(0.0, person.ProbabilityFor("Tom", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("London", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("Malmö", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("Lars", 1));
      Assert.That<double>(person.ProbabilityFor("Lars", 0), Is.EqualTo(0.134).Within(0.001));
      Assert.That<double>(person.ProbabilityFor("Malmø", 1), Is.EqualTo(0.134).Within(0.001));
      Assert.That<double>(person.ProbabilityFor("Petersen", 2), Is.EqualTo(0.067).Within(0.001));
    }


    [Test]
    public void EntityLookupIsCaseInsensitive()
    {
      // Arrange
      string cfg = @"
! entities (city)
{
  ""Århus"",
  ""London"",
  ""Søndre Omme"",
  ""Nørre Omme""
}
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.SetupComplete();

      // total word count = twc = 6
      // P(city,0) = count(city,0) / twc = 4 / 6 = 0.667
      // P(city,1) = count(city,1) / twc = 2 / 6 = 0.333
      // P(århus,0|city,0) = count(århus,0) / count(city,0) = 1 / 4 = 0.250
      // P(city,0|århus,0) = P(city,0) * P(århus,0|city,0) = 0.667 * 0.250 = 0.167
      // P(omme,1|city,1) = count(omme,1) / count(city,1) = 2 / 2 = 1
      // P(city,1|omme,1) = P(city,1) * P(omme,1|city,1) = 0.333 * 1 = 0.333

      EntityClass city = kb.EntityManager.EntityClasses["city"];
      Assert.That<double>(city.ProbabilityFor("århus", 0), Is.EqualTo(0.167).Within(0.001));
      Assert.That<double>(city.ProbabilityFor("OmMe", 1), Is.EqualTo(0.333).Within(0.001));
    }


    [Test]
    public void EntityDefinitionIgnoresPunctuation()
    {
      // Arrange
      string cfg = @"
! entities (organization)
{
  ""Zimmers"",
  ""Acme Inc."",
  ""Blue-Whale"",
  ""National Health Service""
}
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.SetupComplete();

      EntityClass organization = kb.EntityManager.EntityClasses["organization"];
      Assert.Greater(organization.ProbabilityFor("Acme", 0), 0.01);
      Assert.Greater(organization.ProbabilityFor("Inc", 1), 0.01);
      Assert.Greater(organization.ProbabilityFor("blue", 0), 0.01);
      Assert.Greater(organization.ProbabilityFor("whale", 1), 0.01);
      Assert.Greater(organization.ProbabilityFor("service", 2), 0.01);
    }


    [Test]
    public void CanJoinEntityNames()
    {
      // Arrange
      var x = new Tuple<string, ZTokenSequence>[]
      {
        new Tuple<string, ZTokenSequence>("zimmers", new ZTokenSequence { new ZToken("zimmers", "organization") }),
        new Tuple<string, ZTokenSequence>("acme INC", new ZTokenSequence { new ZToken("acme INC", "organization") }),
        new Tuple<string, ZTokenSequence>(
          "where is Blue whale located", 
          new ZTokenSequence { new ZToken("where"), new ZToken("is"), new ZToken("Blue whale", "organization"), new ZToken("located") }),
        new Tuple<string, ZTokenSequence>(
          "Here is national health insurance",
          new ZTokenSequence { new ZToken("Here"), new ZToken("is"), new ZToken("national health insurance", "organization") }),
        new Tuple<string, ZTokenSequence>(
          "Here is national health insurance and national health and acme inc and some more",
          new ZTokenSequence {
            new ZToken("Here"),
            new ZToken("is"),
            new ZToken("national health insurance", "organization"),
            new ZToken("and"),
            new ZToken("national health", "organization"),
            new ZToken("and"),
            new ZToken("acme inc", "organization"),
            new ZToken("and"),
            new ZToken("some"),
            new ZToken("more")}),
      };

      string cfg = @"
! entities (organization)
{
  ""Zimmers"",
  ""Acme Inc."",
  ""Blue Whale"",
  ""National Health"",
  ""National Health Insurance""
}
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.SetupComplete();

      foreach (var src in x)
      {
        ZTokenSequence zinput = new ZTokenSequence(src.Item1.Split(' ').Select(i => new ZToken(i)));
        ZTokenSequenceList result = new ZTokenSequenceList();
        kb.EntityManager.FindEntities(zinput, result);

        ZTokenSequence expectedOutput = src.Item2;

        Assert.AreEqual(1, result.Count, "Testing: " + zinput.ToString());
        Assert.AreEqual(expectedOutput.Count, result[0].Count);
        for (int i = 0; i < expectedOutput.Count; ++i)
        {
          Assert.AreEqual(expectedOutput[i].OriginalText, result[0][i].OriginalText, $"Testing: {src.Item1}");
          Assert.AreEqual(expectedOutput[i].EntityClass, result[0][i].EntityClass, $"Testing: {src.Item1}");
        }
      }
    }


    [Test]
    public void CanDefineCartesianProductForEntities()
    {
      // Arrange
      string cfg = @"
! entities (person)
{
  {
    ""Helle"",
    ""Lars""
  },
  {
    ""Hansen"",
    ""Jensen""
  }
}
";
      // Act
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.SetupComplete();

      // Assert
      Assert.True(kb.EntityManager.EntityClasses.ContainsKey("person"));

      EntityClass ec = kb.EntityManager.EntityClasses["person"];
      Assert.AreEqual(2, ec.LongestWordCount);

      ZTokenSequence zinput = new ZTokenSequence(new ZToken[] { new ZToken("Helle"), new ZToken("Jensen") });

      // Act
      ZTokenSequence result = kb.EntityManager.CalculateLabels(zinput);

      // Assert
      Assert.AreEqual(1, result.Count);
      Assert.IsNotNull(result[0]);
      Assert.AreEqual("Helle Jensen", result[0].OriginalText);
      Assert.AreEqual(ZToken.TokenType.Entity, result[0].Type);
      Assert.AreEqual("person", result[0].EntityClass);
    }


    [Test]
    public void CanDefineEntitesUsingRegex()
    {
      // Arrange
      string cfg = @"
! concept surnames = Helle, Kaj
! concept lastnames = Nielsen, Berg

! entities (person)
{
  > %surnames %surnames? %lastnames %lastnames?
}
";
      // Act
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.SetupComplete();

      // Assert
      Assert.True(kb.EntityManager.EntityClasses.ContainsKey("person"));
    }
  }
}
