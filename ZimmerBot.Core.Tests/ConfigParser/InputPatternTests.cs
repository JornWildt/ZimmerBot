using System.Collections.Generic;
using NUnit.Framework;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class InputPatternTests : TestHelper
  {
    [Test]
    public void StringMatchIsCaseInsensitive()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> AAA bbb cCc
: ok
");

      VerifyMatch(seq, "aaa BBB   Ccc");
    }

    [Test]
    public void CanParseSequence()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa bbb ccc
: ok
");

      Assert.AreEqual(3, seq.Sequence.Count);
      Assert.AreEqual("aaa", ((LiteralWRegex)seq.Sequence[0]).Literal);
      Assert.AreEqual("bbb", ((LiteralWRegex)seq.Sequence[1]).Literal);
      Assert.AreEqual("ccc", ((LiteralWRegex)seq.Sequence[2]).Literal);

      VerifyMatch(seq, "aaa bbb   ccc");
      VerifyNoMatch(seq, "aaa bbb ddd ccc");
      VerifyNoMatch(seq, "aaa bbb");
      VerifyNoMatch(seq, "aaa");
    }


    [Test]
    public void CanParseChoice()
    {
      ChoiceWRegex ch = ParseRuleAndGetRootWRegex<ChoiceWRegex>(@"
> aaa | bbb
: ok
");

      Assert.IsInstanceOf<LiteralWRegex>(ch.Choices[0]);
      Assert.AreEqual("aaa", ((LiteralWRegex)ch.Choices[0]).Literal);
      Assert.IsInstanceOf<LiteralWRegex>(ch.Choices[1]);
      Assert.AreEqual("bbb", ((LiteralWRegex)ch.Choices[1]).Literal);

      VerifyMatch(ch, "aaa");
      VerifyMatch(ch, "bbb");
      VerifyNoMatch(ch, "ccc");
      VerifyNoMatch(ch, "ccc aaa");
    }


    [Test]
    public void CanParseSequenceAndChoice()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa bbb | ccc
: ok
");
      Assert.AreEqual(2, seq.Sequence.Count);
      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<ChoiceWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<LiteralWRegex>(((ChoiceWRegex)seq.Sequence[1]).Choices[0]);
      Assert.IsInstanceOf<LiteralWRegex>(((ChoiceWRegex)seq.Sequence[1]).Choices[1]);

      VerifyMatch(seq, "aaa bbb");
      VerifyMatch(seq, "aaa ccc");
      VerifyNoMatch(seq, "aaa");
      VerifyNoMatch(seq, "ccc");
      VerifyNoMatch(seq, "aaa ddd");
    }


    [Test]
    public void CanParseRepetition()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa * bbb
: ok
");

      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<GroupWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[2]);

      VerifyMatch(seq, "aaa oiqw bbb", new Dictionary<string, string> { { "1", "oiqw" } });
      VerifyNoMatch(seq, "aaa oiqw bbbx");
    }


    [Test]
    public void CanParseOneOrMoreRepetition()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa + bbb
: ok
");

      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<GroupWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[2]);

      VerifyMatch(seq, "aaa oiqw bbb", new Dictionary<string, string> { { "1", "oiqw" } });
      VerifyNoMatch(seq, "aaa bbb");
      VerifyNoMatch(seq, "aaa oiqw bbbx");
    }


    [Test]
    public void CanParseAndMatchGroup()
    {
      ChoiceWRegex ch = ParseRuleAndGetRootWRegex<ChoiceWRegex>(@"
> (aaa bbb) | ccc
: ok
");
      Assert.IsInstanceOf<GroupWRegex>(ch.Choices[0]);
      GroupWRegex left = (GroupWRegex)ch.Choices[0];
      Assert.AreEqual(2, ((SequenceWRegex)left.Sub).Sequence.Count);
      Assert.IsInstanceOf<LiteralWRegex>(ch.Choices[1]);
      Assert.AreEqual("ccc", ((LiteralWRegex)ch.Choices[1]).Literal);

      VerifyMatch(ch, "aaa bbb", new Dictionary<string, string> { { "1", "aaa bbb" } });
      VerifyNoMatch(ch, "aaa ccc");
      VerifyNoMatch(ch, "aaa");
      VerifyMatch(ch, "ccc", new Dictionary<string, string> { { "1", "" } });
      VerifyNoMatch(ch, "aaa ddd");
    }


    [Test]
    public void CanParseAndMatchGroup2()
    {
      ChoiceWRegex ch = ParseRuleAndGetRootWRegex<ChoiceWRegex>(@"
> (aaa bbb) | (ccc)
: ok
");
      Assert.IsInstanceOf<GroupWRegex>(ch.Choices[0]);
      GroupWRegex left = (GroupWRegex)ch.Choices[0];
      Assert.AreEqual(2, ((SequenceWRegex)left.Sub).Sequence.Count);
      Assert.IsInstanceOf<GroupWRegex>(ch.Choices[1]);

      VerifyMatch(ch, "aaa bbb", new Dictionary<string, string> { { "1", "aaa bbb" }, { "2", "" } });
      VerifyNoMatch(ch, "aaa ccc");
      VerifyNoMatch(ch, "aaa");
      VerifyMatch(ch, "ccc", new Dictionary<string, string> { { "1", "" }, { "2", "ccc" } });
      VerifyNoMatch(ch, "aaa ddd");
    }


    [Test]
    public void CanParseZeroOrOne()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa (bbb|ccc)? ddd
: ok
");

      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<GroupWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[2]);

      RepetitionWRegex r = (RepetitionWRegex)((GroupWRegex)seq.Sequence[1]).Sub;
      Assert.AreEqual(0, r.MinCount);
      Assert.AreEqual(1, r.MaxCount);

      VerifyMatch(seq, "aaa ddd", new Dictionary<string, string> { { "1", "" }, { "2", "" } });
      VerifyMatch(seq, "aaa bbb ddd", new Dictionary<string, string> { { "1", "bbb" }, { "2", "bbb" } });
      VerifyMatch(seq, "aaa ccc ddd", new Dictionary<string, string> { { "1", "ccc" }, { "2", "ccc" } });
      VerifyNoMatch(seq, "aaa bbb ccc ddd");
      VerifyNoMatch(seq, "aaa ccc ccc ddd");
      VerifyNoMatch(seq, "aaa bbb");
      VerifyNoMatch(seq, "aaa xxx ddd");
    }


    [Test]
    public void CanParseZeroOrOneWithOneOrMore()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa + bbb?
: ok
");

      Assert.IsInstanceOf<LiteralWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<GroupWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<GroupWRegex>(seq.Sequence[2]);

      RepetitionWRegex r = (RepetitionWRegex)((GroupWRegex)seq.Sequence[2]).Sub;
      Assert.IsInstanceOf<LiteralWRegex>(r.Sub);

      VerifyMatch(seq, "aaa x", new Dictionary<string, string> { { "1", "x" }, { "2", "" } });
      VerifyMatch(seq, "aaa x y", new Dictionary<string, string> { { "1", "x y" }, { "2", "" } });
      VerifyMatch(seq, "aaa y bbb", new Dictionary<string, string> { { "1", "y bbb" }, { "2", "" } });
      VerifyMatch(seq, "aaa yyy zzz bbb", new Dictionary<string, string> { { "1", "yyy zzz bbb" }, { "2", "" } });
      VerifyMatch(seq, "aaa bbb bbb", new Dictionary<string, string> { { "1", "bbb bbb" }, { "2", "" } });
      VerifyMatch(seq, "aaa ooo bbb xxxx", new Dictionary<string, string> { { "1", "ooo bbb xxxx" }, { "2", "" } });
      VerifyNoMatch(seq, "qqq o");
      VerifyNoMatch(seq, "aaa");

    }


    [Test]
    public void CanMatchDoubleSequence()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> take (the|that)? +
: ok
");
      VerifyMatch(seq, "take the ball", new Dictionary<string, string> { { "1", "the" }, { "2", "the" }, { "3", "ball" } });
      VerifyMatch(seq, "take that house", new Dictionary<string, string> { { "1", "that" }, { "2", "that" }, { "3", "house" } });
      VerifyMatch(seq, "take the red house", new Dictionary<string, string> { { "1", "the" }, { "2", "the" }, { "3", "red house" } });
      VerifyMatch(seq, "take house", new Dictionary<string, string> { { "1", "" }, { "2", "" }, { "3", "house" } });
      VerifyMatch(seq, "take red house", new Dictionary<string, string> { { "1", "" }, { "2", "" }, { "3", "red house" } });
      VerifyMatch(seq, "take that", new Dictionary<string, string> { { "1", "" }, { "2", "" }, { "3", "that" } });
      VerifyNoMatch(seq, "take");
    }
  }
}
