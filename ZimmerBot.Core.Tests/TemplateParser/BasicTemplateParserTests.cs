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
      SequenceTemplateToken tokens = ParseTemplate(@"aaa {@} ccc ");
      Assert.AreEqual(3, tokens.Tokens.Count);
      Assert.IsInstanceOf<RedirectTemplateToken>(tokens.Tokens[1]);
      RedirectTemplateToken rt = (RedirectTemplateToken)tokens.Tokens[1];
      Assert.AreEqual(0, rt.Tokens.Tokens.Count);
    }


    [Test]
    public void CanParseNonEmptyRedirect()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa {@ bbb} ccc ");
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
      public string ExpandPlaceholdes(string s)
      {
        return $"(OK: {s})";
      }

      public string Invoke(string s)
      {
        return $"(INV: {s})";
      }
    }


    [Test]
    public void CanInstantiateEmptyRedirectTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa {@} ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("aaa (INV: (OK: )) ccc", output);
    }


    [Test]
    public void CanInstantiateNonEmptyRedirectTemplate()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa {@ bbb} ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("aaa (INV: (OK: bbb)) ccc", output);
    }


    [Test]
    public void CanInstantiateNestedRedirectTemplates()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"aaa {@ say {@ bbb} ok} ccc");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("aaa (INV: (OK: say (INV: (OK: bbb)) ok)) ccc", output);
    }


    [Test]
    public void CanUseStringTemplateBraces()
    {
      SequenceTemplateToken tokens = ParseTemplate(@"Loop: <items:{i | <i.x>}>.");
      TemplateContext context = new TemplateContext(new TX());
      string output = tokens.Instantiate(context);
      Assert.AreEqual("(OK: Loop: <items:{i | <i.x>}>.)", output);
    }
  }
}
