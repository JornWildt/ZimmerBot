using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class BasicConfigParserTests : TestHelper
  {

    [Test]
    public void CanParseSimplestPossibleConfiguration()
    {
      KnowledgeBase kb = new KnowledgeBase();
      Domain d = kb.NewDomain("Test");

      CfgParser.ParseConfigurationString(d, @"
> Dav
: selv hej
");

      Assert.AreEqual(1, d.Rules.Count);
      Assert.IsInstanceOf<WordWRegex>(d.Rules[0].Trigger.Regex);
      WordWRegex seq = (WordWRegex)d.Rules[0].Trigger.Regex;
    }

    [Test]
    public void CanParseMultipleRules()
    {
      KnowledgeBase kb = new KnowledgeBase();
      Domain d = kb.NewDomain("Test");

      CfgParser.ParseConfigurationString(d, @"
### Rule 1
> Dav
: selv hej

### Rule 2
> hej
: hejsa
");
    }

    [Test]
    public void CanParseMultipleInputs()
    {
      KnowledgeBase kb = new KnowledgeBase();
      Domain d = kb.NewDomain("Test");

      CfgParser.ParseConfigurationString(d, @"
### Flere mønstre til samme regel
> Hej
> Dav
: selv hej
");
    }
  }
}
