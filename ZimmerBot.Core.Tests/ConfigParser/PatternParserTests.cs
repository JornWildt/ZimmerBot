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
! pattern (intent = current_weather, type = ""question"", other=a:b)
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
      Assert.AreEqual(3, set.Identifiers.Count);
      Assert.AreEqual("intent", set.Identifiers[0].Key);
      Assert.AreEqual("current_weather", set.Identifiers[0].Value[0]);
      Assert.AreEqual("type", set.Identifiers[1].Key);
      Assert.AreEqual("question", set.Identifiers[1].Value[0]);
      Assert.AreEqual("other", set.Identifiers[2].Key);
      Assert.AreEqual("a", set.Identifiers[2].Value[0]);
      Assert.AreEqual("b", set.Identifiers[2].Value[1]);

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
      Assert.IsNotNull(kb.DefaultTopic.StandardRules);
      Assert.AreEqual(3, kb.DefaultTopic.StandardRules.Count);
      Assert.IsInstanceOf<FuzzyTrigger>(kb.DefaultTopic.StandardRules[0].Trigger);
      Assert.IsInstanceOf<FuzzyTrigger>(kb.DefaultTopic.StandardRules[1].Trigger);
      Assert.IsInstanceOf<FuzzyTrigger>(kb.DefaultTopic.StandardRules[2].Trigger);

      FuzzyTrigger t1 = (FuzzyTrigger)kb.DefaultTopic.StandardRules[0].Trigger;
      FuzzyTrigger t2 = (FuzzyTrigger)kb.DefaultTopic.StandardRules[1].Trigger;
      FuzzyTrigger t3 = (FuzzyTrigger)kb.DefaultTopic.StandardRules[2].Trigger;

      Assert.AreEqual(1, t1.KeyValuePatterns.Count);
      Assert.AreEqual(2, t1.KeyValuePatterns[0].Count);
      Assert.AreEqual("intent", t1.KeyValuePatterns[0][0].Key);
      Assert.AreEqual("current_weather", t1.KeyValuePatterns[0][0].Value);
      Assert.AreEqual("type", t1.KeyValuePatterns[0][1].Key);
      Assert.AreEqual("question", t1.KeyValuePatterns[0][1].Value);
      Assert.AreEqual("loc", t2.KeyValuePatterns[0][2].Key);
      Assert.AreEqual(Constants.StarValue, t2.KeyValuePatterns[0][2].Value);
      Assert.AreEqual("loc", t3.KeyValuePatterns[0][2].Key);
      Assert.AreEqual("Smørum Nedre", t3.KeyValuePatterns[0][2].Value);
    }


    [Test]
    public void CanParseAndExpandConceptsInPatterns()
    {
      string cfg = @"
! concept like = like, enjoy, prefer

! pattern (intent = you_like, type = question)
{
  > do you %like fruits
}
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg, doSetupComplete: false);
      PatternSet set = kb.PatternManager.PatternSets[0];
      Assert.AreEqual(1, set.Patterns.Count);
      Assert.IsNotNull(set.Patterns[0].Expressions);
      Assert.AreEqual(4, set.Patterns[0].Expressions.Count);
      Assert.IsInstanceOf<ConceptPatternExpr>(set.Patterns[0].Expressions[2]);

      ConceptPatternExpr cexpr = (ConceptPatternExpr)set.Patterns[0].Expressions[2];
      Assert.AreEqual("like", cexpr.Word);

      string[] expectedWords = new string[] { "like", "enjoy", "prefer" };

      kb.SetupComplete();
      Assert.AreEqual(3, set.Patterns.Count);
      for (int i=0; i<set.Patterns.Count; ++i)
      {
        Pattern p = set.Patterns[i];
        Assert.AreEqual(4, p.Expressions.Count);
        Assert.IsInstanceOf<WordPatternExpr>(p.Expressions[2]);

        WordPatternExpr wexpr = (WordPatternExpr)p.Expressions[2];
        Assert.AreEqual(expectedWords[i], wexpr.Word);
      }
    }


    [Test]
    public void CanParseAndExpandRecursiveConceptsInPatterns()
    {
      string cfg = @"
! concept like_1 = enjoy, prefer
! concept like = like, %like_1

! pattern (intent = you_like, type = question)
{
  > do you %like fruits
}
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg, doSetupComplete: false);
      PatternSet set = kb.PatternManager.PatternSets[0];

      string[] expectedWords = new string[] { "like", "enjoy", "prefer" };

      kb.SetupComplete();
      Assert.AreEqual(3, set.Patterns.Count);
      for (int i = 0; i < set.Patterns.Count; ++i)
      {
        Pattern p = set.Patterns[i];
        Assert.AreEqual(4, p.Expressions.Count);
        Assert.IsInstanceOf<WordPatternExpr>(p.Expressions[2]);

        WordPatternExpr wexpr = (WordPatternExpr)p.Expressions[2];
        Assert.AreEqual(expectedWords[i], wexpr.Word);
      }
    }
  }
}

