using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.TemplateParser
{
  internal partial class TemplateParser
  {
    public TemplateParser() : base(null) { }

    public void Parse(string s)
    {
      byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
      MemoryStream stream = new MemoryStream(inputBuffer);
      this.Scanner = new TemplateScanner(stream);
      this.Parse();

      string reference = "Template error in: " + s.Substring(0, Math.Min(s.Length,50));
      if (((TemplateScanner)Scanner).Errors != null && ((TemplateScanner)Scanner).Errors.Count > 0)
        throw new ParserException(reference, ((TemplateScanner)Scanner).Errors);
    }


    public SequenceTemplateToken Result { get; internal set; }


    protected SequenceTemplateToken Combine(SequenceTemplateToken sequence, TemplateToken token)
    {
      TemplateToken lastToken = sequence.Tokens.Count > 0
        ? sequence.Tokens[sequence.Tokens.Count-1]
        : null;

      if (lastToken is TextTemplateToken && token is TextTemplateToken)
      {
        ((TextTemplateToken)lastToken).AppendText(((TextTemplateToken)token).Text);
      }
      else
      {
        sequence.Tokens.Add(token);
      }

      return sequence;
    }
  }
}
