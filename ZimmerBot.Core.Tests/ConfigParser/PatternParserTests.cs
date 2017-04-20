using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Patterns;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class PatternParserTests : TestHelper
  {
    [Test]
    public void CanParsePatternDefinition()
    {
      string cfg = @"
! pattern (intent = current_weather, type = ""question"")
{
  > is it snowing
  > is it raining in {l:location}
  > what is {item}
}
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      Assert.IsNotNull(kb.PatternManager);
      Assert.IsNotNull(kb.PatternManager.PatternSets);
      Assert.AreEqual(1, kb.PatternManager.PatternSets.Count);

      PatternSet set = kb.PatternManager.PatternSets[0];

      Assert.IsNotNull(set.Identifiers);
      Assert.AreEqual(2, set.Identifiers.Count);
      Assert.AreEqual("intent", set.Identifiers[0].Key);
      Assert.AreEqual("current_weather", set.Identifiers[0].Value);
      Assert.AreEqual("type", set.Identifiers[1].Key);
      Assert.AreEqual("question", set.Identifiers[1].Value);

      Assert.IsNotNull(set.Patterns);
      Assert.AreEqual(3, set.Patterns.Count);

      Assert.IsNotNull(set.Patterns[0]);
      Assert.IsNotNull(set.Patterns[0].Expressions);
      Assert.AreEqual(3, set.Patterns[0].Expressions.Count);
      Assert.IsInstanceOf<WordPatternExpr>(set.Patterns[0].Expressions[0]);
      Assert.IsInstanceOf<WordPatternExpr>(set.Patterns[0].Expressions[1]);
      Assert.IsInstanceOf<WordPatternExpr>(set.Patterns[0].Expressions[2]);
      WordPatternExpr expr2 = (WordPatternExpr)set.Patterns[0].Expressions[2];
      Assert.AreEqual("snowing", expr2.Word);

      Assert.IsNotNull(set.Patterns[1]);
      Assert.IsNotNull(set.Patterns[1].Expressions);
      Assert.AreEqual(5, set.Patterns[1].Expressions.Count);
      Assert.IsInstanceOf<WordPatternExpr>(set.Patterns[1].Expressions[0]);
      Assert.IsInstanceOf<EntityPatternExpr>(set.Patterns[1].Expressions[4]);
      EntityPatternExpr expr4 = (EntityPatternExpr)set.Patterns[1].Expressions[4];
      Assert.AreEqual("l", expr4.ParameterName);
      Assert.AreEqual("location", expr4.EntityClass);

      Assert.IsNotNull(set.Patterns[2]);
      Assert.IsNotNull(set.Patterns[2].Expressions);
      Assert.AreEqual(3, set.Patterns[2].Expressions.Count);
      Assert.IsInstanceOf<WordPatternExpr>(set.Patterns[2].Expressions[0]);
      Assert.IsInstanceOf<EntityPatternExpr>(set.Patterns[2].Expressions[2]);
      EntityPatternExpr expr2_2 = (EntityPatternExpr)set.Patterns[2].Expressions[2];
      Assert.AreEqual("item", expr2_2.ParameterName);
      Assert.IsNull(expr2_2.EntityClass);
    }


    [Test]
    public void CanParsePatternMatchingRules()
    {
      string cfg = @"
! pattern (intent = current_weather, type = ""question"")
{
  > is it snowing
  > is it raining in {loc:location}
}

>> { intent = current_weather, type = question }
: The weather is good

>> { intent = current_weather, type = question, loc = * }
: The weather is good in <loc>

>> { intent = current_weather, type = question, loc = ""Smørum Nedre"" }
: The weather is fantastic in Smørum Nedre
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      Assert.IsNotNull(kb.DefaultTopic.PatternRules);
      Assert.AreEqual(3, kb.DefaultTopic.PatternRules.Count);
      Assert.IsNotNull(kb.DefaultTopic.PatternRules[0].KeyValuePattern);
      Assert.AreEqual(2, kb.DefaultTopic.PatternRules[0].KeyValuePattern.Count);
      Assert.AreEqual("intent", kb.DefaultTopic.PatternRules[0].KeyValuePattern[0].Key);
      Assert.AreEqual("current_weather", kb.DefaultTopic.PatternRules[0].KeyValuePattern[0].Value);
      Assert.AreEqual("type", kb.DefaultTopic.PatternRules[0].KeyValuePattern[1].Key);
      Assert.AreEqual("question", kb.DefaultTopic.PatternRules[0].KeyValuePattern[1].Value);
      Assert.AreEqual("loc", kb.DefaultTopic.PatternRules[1].KeyValuePattern[2].Key);
      Assert.AreEqual(Constants.StarValue, kb.DefaultTopic.PatternRules[1].KeyValuePattern[2].Value);
      Assert.AreEqual("loc", kb.DefaultTopic.PatternRules[2].KeyValuePattern[2].Key);
      Assert.AreEqual("Smørum Nedre", kb.DefaultTopic.PatternRules[2].KeyValuePattern[2].Value);
    }
  }
}

