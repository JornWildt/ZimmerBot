using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.TemplateParser;

namespace ZimmerBot.Core.Tests.TemplateParser
{
  [TestFixture]
  public class BasicTemplateParserTests : TestHelper
  {
    [Test]
    public void CanParseBasicTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa bbb ccc ");
      Assert.AreEqual(1, tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[0]);
      TextTemplateToken tt =  (TextTemplateToken)tokens.Tokens[0];
      Assert.AreEqual("aaa bbb ccc ", tt.Text);
    }


    [Test]
    public void CanParseEmptyRedirect()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa <<>> ccc ");
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<RedirectTemplateToken>(tokens.Tokens[1]);
      RedirectTemplateToken rt = (RedirectTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(0, rt.Tokens.Tokens.Count);
    }


    [Test]
    public void CanParseNonEmptyRedirect()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa << bbb>> ccc ");
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<RedirectTemplateToken>(tokens.Tokens[1]);
      RedirectTemplateToken rt = (RedirectTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(1, rt.Tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(rt.Tokens.Tokens[0]);
      TextTemplateToken tt = (TextTemplateToken)rt.Tokens.Tokens[0];
      Assert.AreEqual("bbb", tt.Text);
    }


    [Test]
    public void CanInstantiateSimpleTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa bbb ccc ");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
    }


    public class TX : ITemplateExpander
    {
      public string ExpandPlaceholders(string s)
      {
        return $"(OK: {s})";
      }

      public string ExpandConcept(string concept)
      {
        throw new NotImplementedException();
      }

      public string Invoke(string s)
      {
        return $"(INV: {s})";
      }
    }


    [Test]
    public void CanInstantiateEmptyRedirectTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa <<>> ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("(OK: aaa )(INV: )(OK:  ccc)", output);
    }


    [Test]
    public void CanInstantiateNonEmptyRedirectTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa << bbb>> ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("(OK: aaa )(INV: (OK: bbb))(OK:  ccc)", output);
    }


    [Test]
    public void CanInstantiateNestedRedirectTemplates()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa << say << bbb>> ok>> ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("(OK: aaa )(INV: (OK: say )(INV: (OK: bbb))(OK:  ok))(OK:  ccc)", output);
    }


    [Test]
    public void CanUseStringTemplateBraces()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"Loop: <items:{i | <i.x>}>.");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("(OK: Loop: <items:{i | <i.x>}>.)", output);
    }


    [Test]
    public void CanUseChoose_1()
    {
      SequenceTemplateToken tokens = ParseTemplate("X <(a)> Y");
      Assert.IsNotNull(tokens);
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[0]);
      Assert.AreEqual("X ", ((TextTemplateToken)tokens.Tokens[0]).Text);

      Assert.IsInstanceOf<ChooseTemplateToken>(tokens.Tokens[1]);
      var choose = (ChooseTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(1, choose.Variants.Count);
      Assert.IsInstanceOf<SequenceTemplateToken>(choose.Variants[0]);
      var seq = (SequenceTemplateToken)choose.Variants[0];
      Assert.AreEqual(1, seq.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq.Tokens[0]);
      Assert.AreEqual("a", ((TextTemplateToken)seq.Tokens[0]).Text);

      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[2]);
      Assert.AreEqual(" Y", ((TextTemplateToken)tokens.Tokens[2]).Text);
    }


    [Test]
    public void CanUseChoose_2()
    {
      SequenceTemplateToken tokens = ParseTemplate("X <(a | b)> Y");
      Assert.IsNotNull(tokens);
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[0]);
      Assert.AreEqual("X ", ((TextTemplateToken)tokens.Tokens[0]).Text);

      Assert.IsInstanceOf<ChooseTemplateToken>(tokens.Tokens[1]);
      var choose = (ChooseTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(2, choose.Variants.Count);

      Assert.IsInstanceOf<SequenceTemplateToken>(choose.Variants[0]);
      var seq1 = (SequenceTemplateToken)choose.Variants[0];
      Assert.AreEqual(1, seq1.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq1.Tokens[0]);
      Assert.AreEqual("a ", ((TextTemplateToken)seq1.Tokens[0]).Text);

      var seq2 = (SequenceTemplateToken)choose.Variants[1];
      Assert.AreEqual(1, seq2.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq2.Tokens[0]);
      Assert.AreEqual(" b", ((TextTemplateToken)seq2.Tokens[0]).Text);

      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[2]);
      Assert.AreEqual(" Y", ((TextTemplateToken)tokens.Tokens[2]).Text);
    }


    [Test]
    public void CanUseChoose_3()
    {
      SequenceTemplateToken tokens = ParseTemplate("X <(a | b| c )> Y");
      Assert.IsNotNull(tokens);
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[0]);
      Assert.AreEqual("X ", ((TextTemplateToken)tokens.Tokens[0]).Text);

      Assert.IsInstanceOf<ChooseTemplateToken>(tokens.Tokens[1]);
      var choose = (ChooseTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(3, choose.Variants.Count);

      Assert.IsInstanceOf<SequenceTemplateToken>(choose.Variants[0]);
      var seq1 = (SequenceTemplateToken)choose.Variants[0];
      Assert.AreEqual(1, seq1.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq1.Tokens[0]);
      Assert.AreEqual("a ", ((TextTemplateToken)seq1.Tokens[0]).Text);

      var seq2 = (SequenceTemplateToken)choose.Variants[1];
      Assert.AreEqual(1, seq2.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq2.Tokens[0]);
      Assert.AreEqual(" b", ((TextTemplateToken)seq2.Tokens[0]).Text);

      var seq3 = (SequenceTemplateToken)choose.Variants[2];
      Assert.AreEqual(1, seq3.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(seq3.Tokens[0]);
      Assert.AreEqual(" c ", ((TextTemplateToken)seq3.Tokens[0]).Text);

      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[2]);
      Assert.AreEqual(" Y", ((TextTemplateToken)tokens.Tokens[2]).Text);
    }


    [Test]
    public void CanUseConceptInOutput()
    {
      SequenceTemplateToken tokens = ParseTemplate("I like <(%fruit)>");
      Assert.IsNotNull(tokens);
      Assert.AreEqual(2, tokens.Tokens.Count);
      Assert.IsInstanceOf<TextTemplateToken>(tokens.Tokens[0]);
      Assert.AreEqual("I like ", ((TextTemplateToken)tokens.Tokens[0]).Text);

      Assert.IsInstanceOf<ChooseTemplateToken>(tokens.Tokens[1]);
      var choose = (ChooseTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(1, choose.Variants.Count);
      Assert.IsInstanceOf<SequenceTemplateToken>(choose.Variants[0]);
      var seq = (SequenceTemplateToken)choose.Variants[0];
      Assert.AreEqual(1, seq.Tokens.Count);

      Assert.IsInstanceOf<ConceptTemplateToken>(seq.Tokens[0]);
      Assert.AreEqual("fruit", ((ConceptTemplateToken)seq.Tokens[0]).Concept);
    }
  }
}
