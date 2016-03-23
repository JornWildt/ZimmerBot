using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class OutputTests : TestHelper
  {
    [Test]
    public void CanParseMultipleOneOfOutputLines()
    {
      Rule r = ParseRule(@"
> aa bb
: xxx
: yyy
");
    }


    [Test]
    public void CanUseMatchesInCallStatement()
    {
      Rule r = ParseRule(@"
> aaa (bbb)
! call General.Echo($1)
: ccc <result>
");

      Assert.IsNotNull(r.OutputStatements);
      Assert.AreEqual(2, r.OutputStatements.Count);
      Assert.IsInstanceOf<CallOutputStatment>(r.OutputStatements[0]);
      Assert.IsInstanceOf<TemplateOutputStatement>(r.OutputStatements[1]);

      Reaction reaction = CalculateReaction(r, "aaa bbb");
      Assert.IsNotNull(reaction);

      string result = reaction.GenerateResponse();
      Assert.AreEqual("ccc bbb", result);
    }


    [Test]
    public void CanParseCallStatementWithZeroParameters()
    {
      Rule r = ParseRule(@"
> aaa
! call DateTime.ThisDay()
: ccc <answer>
");

      Assert.IsNotNull(r.OutputStatements);
      Assert.AreEqual(2, r.OutputStatements.Count);
      Assert.IsInstanceOf<CallOutputStatment>(r.OutputStatements[0]);
      Assert.IsInstanceOf<TemplateOutputStatement>(r.OutputStatements[1]);

      Reaction reaction = CalculateReaction(r, "aaa");
      Assert.IsNotNull(reaction);

      string result = reaction.GenerateResponse();
      StringAssert.IsMatch("ccc ..+", result);
    }


    [Test]
    public void CanUseMatchInOutputTemplate()
    {
      Rule r = ParseRule(@"
> (aaa)
: ccc <1>
");

      string result = GetResponseFrom(r, "aaa");
      StringAssert.IsMatch("ccc aaa", result);
    }
  }
}
