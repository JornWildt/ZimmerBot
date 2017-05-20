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
    public void CanMatchEntityNames()
    {
      // Arrange
      var x = new Tuple<string, ZTokenSequence, int, int>[]
      {
        new Tuple<string, ZTokenSequence, int, int>("zimmers", new ZTokenSequence { new ZToken("zimmers", "organization") }, 1, 0),
        new Tuple<string, ZTokenSequence, int, int>("acme INC", new ZTokenSequence { new ZToken("acme INC", "organization") }, 1, 0),
        new Tuple<string, ZTokenSequence, int, int>(
          "where is Blue whale located",
          new ZTokenSequence { new ZToken("where"), new ZToken("is"), new ZToken("Blue whale", "organization"), new ZToken("located") },
          1, 0),
        new Tuple<string, ZTokenSequence, int, int>(
          "Here is national health insurance",
          new ZTokenSequence { new ZToken("Here"), new ZToken("is"), new ZToken("national health insurance", "organization") },
          1, 0),
        new Tuple<string, ZTokenSequence, int, int>(
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
            new ZToken("more")},
          7, 2),
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

        // Item3 is number of variations
        Assert.AreEqual(src.Item3, result.Count, "Testing: " + zinput.ToString());
        // Item4 indicates which of the alternative results that should be used
        ZTokenSequence res = result[src.Item4];
        Assert.AreEqual(expectedOutput.Count, res.Count);
        for (int i = 0; i < expectedOutput.Count; ++i)
        {
          Assert.AreEqual(expectedOutput[i].OriginalText, res[i].OriginalText, $"Testing: {src.Item1}");
          Assert.AreEqual(expectedOutput[i].EntityClass, res[i].EntityClass, $"Testing: {src.Item1}");
        }
      }
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
