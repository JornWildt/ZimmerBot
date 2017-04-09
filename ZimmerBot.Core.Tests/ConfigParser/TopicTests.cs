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
  public class TopicTests : TestHelper
  {
    [Test]
    public void CanParseTopic()
    {
      // Arrange
      string cfg = @"
! topic Zombies (zombie, dead, rotten)
{
  > now what
  : Fly, you fools, fly!
}
";
      // Act
      Topic t = ParseTopic(cfg);

      // Assert
      Assert.IsNotNull(t);
      Assert.AreEqual("Zombies", t.Name);
      Assert.IsNotNull(t.TriggerWords);
      Assert.AreEqual(3, t.TriggerWords.Count);
      Assert.AreEqual("zombie", t.TriggerWords[0]);
      Assert.AreEqual("dead", t.TriggerWords[1]);
      Assert.AreEqual("rotten", t.TriggerWords[2]);
      Assert.IsNotNull(t.Rules);
      Assert.AreEqual(1, t.Rules.Count);
      Assert.IsInstanceOf<WordRegex.SequenceWRegex>(t.Rules[0].Trigger.Regex.Expr);
      Assert.AreEqual(1, t.Rules[0].Statements.Count);
    }
  }
}
