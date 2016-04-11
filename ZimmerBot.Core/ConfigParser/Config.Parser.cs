using System.Collections.Generic;
using System.IO;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigParser
  {
    protected KnowledgeBase KnowledgeBase { get; set; }


    public ConfigParser(KnowledgeBase kb) 
      : base(null)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      KnowledgeBase = kb;
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


    protected void RegisterConcept(string name, List<List<string>> patterns)
    {
      Concept c = KnowledgeBase.AddConcept(name, patterns);
    }


    protected void RDFImport(string filename)
    {
      KnowledgeBase.MemoryStore.LoadFromFile(filename);
    }


    protected void RDFPrefix(string prefix, string url)
    {
      KnowledgeBase.MemoryStore.DeclarePrefix(prefix, url);
    }


    protected Rule AddRule(string label, WRegex pattern, List<RuleModifier> modifiers, List<Statement> outputs)
    {
      Rule r = KnowledgeBase.AddRule(pattern);
      if (label != null)
        r.WithLabel(label);
      if (modifiers != null)
        foreach (var m in modifiers)
          m.Invoke(r);
      if (outputs != null)
        r.WithOutputStatements(outputs);
      return r;
    }


    protected WRegex CombineSequence(WRegex left, WRegex right)
    {
      if (left == null && right != null)
        return right;

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
  }
}
