using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class TopicTests : TestHelper
  {
    [Test]
    public void CanParseTopic()
    {
      // Arrange
      string cfg = @"
! topic Zombies
{
  > now what
  : Run, you fools, run!
}
";
      // Act
      Topic t = ParseTopic(cfg, "Zombies");

      // Assert
      Assert.IsNotNull(t);
      Assert.AreEqual("Zombies", t.Name);
      Assert.IsNotNull(t.StandardRules);
      Assert.AreEqual(1, t.StandardRules.Count);
      Assert.IsInstanceOf<RegexTrigger>(t.StandardRules[0].Trigger);

      RegexTrigger trigger = (RegexTrigger)t.StandardRules[0].Trigger;
      Assert.IsInstanceOf<WordRegex.SequenceWRegex>(trigger.Regex.Expr);
      Assert.AreEqual(1, t.StandardRules[0].Statements.Count);
    }


    [Test]
    public void CanParseTopicWithOutputSequence()
    {
      // Arrange
      string cfg = @"
! topic Zombies
{
  T> Run!
  +: Now!
}
";
      // Act
      Topic t = ParseTopic(cfg, "Zombies");

      // Assert
      Assert.IsNotNull(t);
      Assert.AreEqual("Zombies", t.Name);
      Assert.IsNotNull(t.StandardRules);
      Assert.AreEqual(0, t.StandardRules.Count);
      Assert.AreEqual(1, t.TopicRules.Count);
      Assert.IsInstanceOf<OutputTemplateStatement>(t.TopicRules[0].Statements[0]);
    }
  }
}
