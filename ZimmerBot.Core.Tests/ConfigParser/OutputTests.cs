using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class OutputTests : TestHelper
  {
    [Test]
    public void CanParseMultipleOneOfOutputLines()
    {
      StandardRule r = ParseRule(@"
> aa bb
: xxx
: yyy
");

      Assert.AreEqual(2, r.Statements.Count);
    }


    [Test]
    public void CanUseMatchesInCallStatement()
    {
      StandardRule r = ParseRule(@"
> aaa (bbb)
! call General.Echo($1)
: ccc <result>
");

      Assert.IsNotNull(r.Statements);
      Assert.AreEqual(2, r.Statements.Count);
      Assert.IsInstanceOf<CallStatment>(r.Statements[0]);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[1]);

      IList<Reaction> reactions = CalculateReactions(r, "aaa bbb");
      Assert.IsNotNull(reactions);

      string result = GetResponseFrom(reactions);
      Assert.AreEqual("ccc bbb", result);
    }


    [Test]
    public void CanParseCallStatementWithZeroParameters()
    {
      StandardRule r = ParseRule(@"
> aaa
! call DateTime.Time()
: ccc <answer>
");

      Assert.IsNotNull(r.Statements);
      Assert.AreEqual(2, r.Statements.Count);
      Assert.IsInstanceOf<CallStatment>(r.Statements[0]);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[1]);

      IList<Reaction> reactions = CalculateReactions(r, "aaa");
      Assert.IsNotNull(reactions);

      string result = GetResponseFrom(reactions);
      StringAssert.IsMatch("ccc ..+", result);
    }


    [Test]
    public void CanUseMatchInOutputTemplate()
    {
      StandardRule r = ParseRule(@"
> (aaa)
: ccc <1>
");

      string result = GetResponseFrom(r, "aaa");
      StringAssert.IsMatch("ccc aaa", result);
    }


    [Test]
    public void CanUse_SequenceOneOrMore_MatchInOutputTemplate()
    {
      StandardRule r = ParseRule(@"
> aaa + bbb
: ccc <1>
");

      string result = GetResponseFrom(r, "aaa xxx bbb");
      StringAssert.IsMatch("ccc xxx", result);

      result = GetResponseFrom(r, "aaa bbb");
      Assert.IsNull(result);
    }


    [Test]
    public void CanUse_SequenceOneOrMore_MatchInOutputTemplate2()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
> are you +
: would you like me to be <1>

> *
: Duh?
");

      TriggerEvaluationContext context = BuildEvaluationContextFromInput(kb, "are you a computer");
      ReactionSet reactions = new ReactionSet();
      kb.FindMatchingReactions(context, reactions);

      Assert.AreEqual(1, reactions.Count);

      string result = reactions[0].GenerateResponse().Aggregate((a, b) => a + "\n" + b);

      StringAssert.IsMatch("would you like me to be a computer", result);
    }


    [Test]
    public void CanMakeMultiLineOutput()
    {
      StandardRule r = ParseRule(@"
> aaa
: ccc\
ddd");

      string result = GetResponseFrom(r, "aaa");
      StringAssert.IsMatch("ccc\r\nddd", result);
    }


    [Test]
    public void CanSpecifyOutputTemplate()
    {
      StandardRule r = ParseRule(@"
> aaa
: bbb
{xxx}: ccc
");

      Assert.AreEqual(2, r.Statements.Count);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[0]);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[1]);
      OutputTemplateStatement ts = (OutputTemplateStatement)r.Statements[0];
      Assert.AreEqual("default", ts.Template.TemplateName);
      Assert.AreEqual(1, ts.Template.Outputs.Count);
      Assert.AreEqual("bbb", ts.Template.Outputs[0]);

      ts = (OutputTemplateStatement)r.Statements[1];
      Assert.AreEqual("xxx", ts.Template.TemplateName);
      Assert.AreEqual(1, ts.Template.Outputs.Count);
      Assert.AreEqual("ccc", ts.Template.Outputs[0]);

      string result = GetResponseFrom(r, "aaa");
      StringAssert.IsMatch("bbb", result);
    }
  }
}
