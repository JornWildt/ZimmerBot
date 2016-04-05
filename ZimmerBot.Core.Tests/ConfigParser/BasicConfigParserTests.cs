﻿using NUnit.Framework;
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

      CfgParser.ParseConfigurationString(kb, @"
> Dav
: selv hej
");

      Assert.AreEqual(1, kb.Rules.Count);
      Assert.IsInstanceOf<WordWRegex>(kb.Rules[0].Trigger.Regex);
      WordWRegex seq = (WordWRegex)kb.Rules[0].Trigger.Regex;
    }

    [Test]
    public void CanParseMultipleRules()
    {
      KnowledgeBase kb = new KnowledgeBase();

      CfgParser.ParseConfigurationString(kb, @"
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

      CfgParser.ParseConfigurationString(kb, @"
### Flere mønstre til samme regel
> Hej
> Dav
: selv hej
");
    }
  }
}
