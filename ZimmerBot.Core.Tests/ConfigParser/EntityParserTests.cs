using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

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
  Århus,
  London,
  Søndre Omme,
  Nørre Omme
}

! entities (person)
{
  Lise Nielsen,
  Lars Malmø,
  Peter Malmø,
  Lars Bo Petersen
}
";

      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      kb.Run();

      Assert.AreEqual(2, kb.EntityManager.EntityClasses.Count);
      Assert.IsTrue(kb.EntityManager.EntityClasses.ContainsKey("city"));
      Assert.IsTrue(kb.EntityManager.EntityClasses.ContainsKey("person"));

      // total word count = twc = 15
      // P(city,0) = count(city,0) / twc = 4/15 = 
      // P(city,1) = count(city,1) / twc = 2/15 = 
      EntityClass city = kb.EntityManager.EntityClasses["city"];
      Assert.AreEqual(2, city.LongestWordCount);
      Assert.AreEqual(6, city.TotalNumberOfWords);
      Assert.AreEqual(0.0, city.ProbabilityFor("Berlin", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Lise", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Omme", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Århus", 1));
      Assert.AreEqual(0.0, city.ProbabilityFor("Århus", 0));
      Assert.AreEqual(0.0, city.ProbabilityFor("Omme", 1));

      EntityClass person = kb.EntityManager.EntityClasses["person"];
      Assert.AreEqual(3, person.LongestWordCount);
      Assert.AreEqual(9, person.TotalNumberOfWords);
      Assert.AreEqual(0.0, person.ProbabilityFor("Tom", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("London", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("Malmö", 0));
      Assert.AreEqual(0.0, person.ProbabilityFor("Lars", 1));
    }
  }
}
