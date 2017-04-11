using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class OutputSequenceTests : TestHelper
  {
    [Test]
    public void CanParseOutputSequence()
    {
      StandardRule r = ParseRule(@"
> Yo
: First sentence. 
+: Second sentence\
with a newline.
+: Third sentence.");

      Assert.AreEqual(1, r.Statements.Count);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[0]);

      OutputTemplateStatement s = (OutputTemplateStatement)r.Statements[0];
      Assert.AreEqual(3, s.Template.Outputs.Count);
      Assert.AreEqual("default", s.Template.TemplateName);
      Assert.AreEqual("First sentence.", s.Template.Outputs[0]);
      Assert.AreEqual("Second sentence\nwith a newline.", s.Template.Outputs[1]);
      Assert.AreEqual("Third sentence.", s.Template.Outputs[2]);
    }


    [Test]
    public void CanParseNamedOutputSequence()
    {
      StandardRule r = ParseRule(@"
> Yo
{xxx}: First sentence. 
+: Second sentence\
with a newline.
+: Third sentence.");

      Assert.AreEqual(1, r.Statements.Count);
      Assert.IsInstanceOf<OutputTemplateStatement>(r.Statements[0]);

      OutputTemplateStatement s = (OutputTemplateStatement)r.Statements[0];
      Assert.AreEqual(3, s.Template.Outputs.Count);
      Assert.AreEqual("xxx", s.Template.TemplateName);
      Assert.AreEqual("First sentence.", s.Template.Outputs[0]);
      Assert.AreEqual("Second sentence\nwith a newline.", s.Template.Outputs[1]);
      Assert.AreEqual("Third sentence.", s.Template.Outputs[2]);
    }
  }
}
