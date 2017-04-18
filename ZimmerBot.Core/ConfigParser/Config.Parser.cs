using System.Collections.Generic;
using System.IO;
using CuttingEdge.Conditions;
using log4net;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigParser
  {
    private static ILog Logger = LogManager.GetLogger(typeof(ConfigParser));

    protected KnowledgeBase KnowledgeBase { get; set; }

    protected string CurrentTopic { get; set; }


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


    protected void StartTopic(string name)
    {
      CurrentTopic = name;
      KnowledgeBase.AddTopic(name);
    }


    protected void FinalizeTopic(string name)
    {
      Condition.Requires(name, nameof(name)).IsEqualTo(CurrentTopic);
      CurrentTopic = null;
    }


    protected void RegisterEntities(string className, List<string> entityNames)
    {
      KnowledgeBase.RegisterEntityClass(className, entityNames);
    }


    protected WRegexBase BuildConceptWRegex(string s)
    {
      if (s == "%ENTITY")
      {
        return new GroupWRegex(
          new ChoiceWRegex(
            new WildcardWRegex(),
            new EntityWRegex()));
      }
      else
        return new ConceptWRegex(KnowledgeBase, s);
    }

    protected void RegisterEventHandler(string e, List<Statement> statements)
    {
      KnowledgeBase.RegisterEventHandler(e, statements);
    }


    protected void RDFImport(string filename)
    {
      KnowledgeBase.MemoryStore.LoadFromFile(filename);
    }


    protected void RDFPrefix(string prefix, string url)
    {
      KnowledgeBase.MemoryStore.DeclarePrefix(prefix, url);
    }


    protected void RDFEntities(string sparql)
    {
      KnowledgeBase.RegisterSparqlForEntities(sparql);
      //System.Console.WriteLine("Entities loaded from {0}", sparql);
    }


    protected StandardRule AddRule(string label, List<WRegexBase> patterns, List<RuleModifier> modifiers, List<Statement> statements)
    {
      return KnowledgeBase.AddRule(label, CurrentTopic, patterns, modifiers, statements);
    }


    protected TopicRule AddTopicRule(string label, OutputTemplate output, List<Statement> statements)
    {
      statements.Insert(0, new OutputTemplateStatement(output));
      return KnowledgeBase.AddTopicRule(label, CurrentTopic, statements);
    }


    protected WRegexBase CombineSequence(WRegexBase left, WRegexBase right)
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
        foreach (WRegexBase r in ((SequenceWRegex)right).Sequence)
          ((SequenceWRegex)left).Add(r);
        return left;
      }
    }
  }
}
