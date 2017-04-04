using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Parser
{
  internal partial class ChatParser
  {
    public ChatParser() : base(null) { }

    public void Parse(string s)
    {
      byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
      MemoryStream stream = new MemoryStream(inputBuffer);
      this.Scanner = new ChatScanner(stream);
      this.Parse();
    }

    protected void AddStatement(Statement stmt)
    {
    }

    public ZStatementSequence Result { get; internal set; }
  }
}
