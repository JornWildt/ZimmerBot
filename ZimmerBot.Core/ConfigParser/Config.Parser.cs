using System.Collections.Generic;
using System.IO;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigParser
  {
    protected Domain Domain { get; set; }


    public ConfigParser(Domain d) 
      : base(null)
    {
      Condition.Requires(d, nameof(d)).IsNotNull();
      Domain = d;
    }


    public void Parse(string s)
    {
      byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
      MemoryStream stream = new MemoryStream(inputBuffer);
      Parse(stream, "string input");
    }


    public void Parse(Stream s, string filename)
    {
      this.Scanner = new ConfigScanner(s);
      this.Parse();
      if (((ConfigScanner)Scanner).Errors != null && ((ConfigScanner)Scanner).Errors.Count > 0)
        throw new ParserException(filename, ((ConfigScanner)Scanner).Errors);
    }


    protected void RegisterAbstractions(List<string> words, List<string> keys)
    {
      WordDefinition wd = Domain.DefineWords(words);
      foreach (string k in keys)
        wd.Is(k);
    }


    protected WRegex CombineSequence(WRegex left, WRegex right)
    {
      if (left is SequenceWRegex && !(right is SequenceWRegex))
      {
        ((SequenceWRegex)left).Add(right);
        return left;
      }
      else if (!(left is SequenceWRegex) && !(right is SequenceWRegex))
      {
        SequenceWRegex seq = new SequenceWRegex();
        seq.Add(left);
        seq.Add(right);
        return seq;
      }
      else if (!(left is SequenceWRegex) && right is SequenceWRegex)
      {
        ((SequenceWRegex)right).Insert(0, left);
        return right;
      }
      else // (left is SequenceWRegex && right is SequenceWRegex)
      {
        foreach (WRegex r in ((SequenceWRegex)right).Sequence)
          ((SequenceWRegex)left).Add(r);
        return left;
      }
    }


    protected int TryParseInt(string s)
    {
      int result;
      if (!int.TryParse(s, out result))
        Scanner.yyerror("The value '{0}' is not a valid integer", s);
      return result;
    }


    protected double TryParseDouble(string s)
    {
      double result;
      if (!double.TryParse(s, out result))
        Scanner.yyerror("The value '{0}' is not a valid number", s);
      return result;
    }
  }
}
