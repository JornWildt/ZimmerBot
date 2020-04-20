using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using VDS.RDF;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class WordDefinitionManager
  {
    protected KnowledgeBase KnowledgeBase { get; set; }

    protected List<WordDefinition> Definitions { get; set; }

    protected NodeFactory NodeFactory { get; set; }


    public WordDefinitionManager(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      KnowledgeBase = kb;
      Definitions = new List<WordDefinition>();
      NodeFactory = new NodeFactory();
    }


    public void RegisterDefinitions(List<string> mainClasses, List<WordDefinition> definitions)
    {
      Condition.Requires(mainClasses, nameof(mainClasses)).IsNotNull();
      Condition.Requires(definitions, nameof(definitions)).IsNotEmpty();

      foreach (WordDefinition w in definitions)
      {
        w.Classes = mainClasses;
        Definitions.Add(w);
      }
    }


    public void SetupComplete(RDFStore store)
    {
      foreach (var word in Definitions)
      {
        DefineRdfsClass(word, store);
        RegisterRdfData(word, store);

        if (SpellChecker.IsInitialized)
          RegisterSpellChecker(word);
      }
    }


    static readonly Uri RdfType = UrlConstants.Rdf("type");

    static readonly Uri RdfsLabel = UrlConstants.Rdfs("label");

    static readonly Uri RdfsClass = UrlConstants.Rdfs("Class");

    static readonly Uri KnownBy = UrlConstants.PropertyUrl("knownby");


    private void DefineRdfsClass(WordDefinition word, RDFStore store)
    {
      foreach (string c in word.Classes)
      {
        Uri subject = UrlConstants.ResourceUrl(c);
        store.Insert(
          NodeFactory.CreateUriNode(subject), 
          NodeFactory.CreateUriNode(RdfType), 
          NodeFactory.CreateUriNode(RdfsClass),
          RDFStore.StaticStoreName);
        store.Insert(
          NodeFactory.CreateUriNode(subject), 
          NodeFactory.CreateUriNode(RdfsLabel), 
          c.ToLiteral(NodeFactory),
          RDFStore.StaticStoreName);
      }
    }


    private void RegisterRdfData(WordDefinition word, RDFStore store)
    {
      // Create subject identifier from word
      string propId = StringUtility.Word2Identifier(word.Id);
      Uri subject = UrlConstants.ResourceUrl(propId);

      // Define word as of type classes
      foreach (string c in word.Classes)
      {
        Uri type = UrlConstants.ResourceUrl(c);
        store.Insert(
          NodeFactory.CreateUriNode(subject), 
          NodeFactory.CreateUriNode(RdfType), 
          NodeFactory.CreateUriNode(type),
          RDFStore.StaticStoreName);
      }

      // Define word as label for itself
      if (word.Word != null)
      {
        store.Insert(
          NodeFactory.CreateUriNode(subject), 
          NodeFactory.CreateUriNode(RdfsLabel), 
          word.Word.ToLiteral(NodeFactory),
          RDFStore.StaticStoreName);

        // Define word and alternatives as "knownby" for indexing
        store.Insert(
          NodeFactory.CreateUriNode(subject), 
          NodeFactory.CreateUriNode(KnownBy), 
          word.Word.ToLower().ToLiteral(NodeFactory),
          RDFStore.StaticStoreName);

        foreach (string alt in word.Alternatives)
          store.Insert(
            NodeFactory.CreateUriNode(subject), 
            NodeFactory.CreateUriNode(KnownBy), 
            alt.ToLower().ToLiteral(NodeFactory),
            RDFStore.StaticStoreName);
      }

      // Define all properties associated with word
      foreach (var prop in word.RdfDefinitions)
      {
        foreach (var value in prop.Values)
        {
          store.Insert(
            NodeFactory.CreateUriNode(subject), 
            prop.Key.BuildRdfNode(NodeFactory),
            value.BuildRdfNode(NodeFactory),
            RDFStore.StaticStoreName);
        }
      }
    }

    private void RegisterSpellChecker(WordDefinition word)
    {
      if (word.Word != null)
      {
        SpellChecker.AddWord(word.Word);
        foreach (string alt in word.Alternatives)
          SpellChecker.AddWord(alt);
      }
    }
  }
}
