using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }


    public SequenceTemplateToken Result { get; internal set; }
  }
}
