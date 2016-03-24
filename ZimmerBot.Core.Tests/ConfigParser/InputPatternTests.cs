using NUnit.Framework;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class InputPatternTests : TestHelper
  {
    [Test]
    public void CanParseSequence()
    {
      SequenceWRegex seq = ParseRuleAndGetRootWRegex<SequenceWRegex>(@"
> aaa bbb ccc
: ok
");

      Assert.AreEqual(3, seq.Sequence.Count);
      Assert.AreEqual("aaa", ((WordWRegex)seq.Sequence[0]).Word);
      Assert.AreEqual("bbb", ((WordWRegex)seq.Sequence[1]).Word);
      Assert.AreEqual("ccc", ((WordWRegex)seq.Sequence[2]).Word);

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

      Assert.IsInstanceOf<WordWRegex>(ch.Left);
      Assert.AreEqual("aaa", ((WordWRegex)ch.Left).Word);
      Assert.IsInstanceOf<WordWRegex>(ch.Right);
      Assert.AreEqual("bbb", ((WordWRegex)ch.Right).Word);

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
      Assert.IsInstanceOf<WordWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<ChoiceWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<WordWRegex>(((ChoiceWRegex)seq.Sequence[1]).Left);
      Assert.IsInstanceOf<WordWRegex>(((ChoiceWRegex)seq.Sequence[1]).Right);

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

      Assert.IsInstanceOf<WordWRegex>(seq.Sequence[0]);
      Assert.IsInstanceOf<RepetitionWRegex>(seq.Sequence[1]);
      Assert.IsInstanceOf<WordWRegex>(seq.Sequence[2]);

      VerifyMatch(seq, "aaa oiqw bbb");
      VerifyNoMatch(seq, "aaa oiqw bbbx");
    }


    [Test]
    public void CanParseGroup()
    {
      ChoiceWRegex ch = ParseRuleAndGetRootWRegex<ChoiceWRegex>(@"
> (aaa bbb) | ccc
: ok
");
      Assert.IsInstanceOf<GroupWRegex>(ch.Left);
      GroupWRegex left = (GroupWRegex)ch.Left;
      Assert.AreEqual(2, ((SequenceWRegex)left.Sub).Sequence.Count);
      Assert.IsInstanceOf<WordWRegex>(ch.Right);
      Assert.AreEqual("ccc", ((WordWRegex)ch.Right).Word);

      VerifyMatch(ch, "aaa bbb");
      VerifyNoMatch(ch, "aaa ccc");
      VerifyNoMatch(ch, "aaa");
      VerifyMatch(ch, "ccc");
      VerifyNoMatch(ch, "aaa ddd");
    }
  }
}
