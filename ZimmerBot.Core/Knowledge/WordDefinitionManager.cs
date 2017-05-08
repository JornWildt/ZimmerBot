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

    protected Dictionary<string, List<WordDefinition>> Definitions { get; set; }

    protected NodeFactory NodeFactory { get; set; }


    public WordDefinitionManager(KnowledgeBase kb)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();

      KnowledgeBase = kb;
      Definitions = new Dictionary<string, List<WordDefinition>>();
      NodeFactory = new NodeFactory();
    }


    public void RegisterWords(string mainClass, List<WordDefinition> definitions)
    {
      Condition.Requires(mainClass, nameof(mainClass)).IsNotNullOrWhiteSpace();
      Condition.Requires(definitions, nameof(definitions)).IsNotEmpty();

      Definitions[mainClass] = definitions;
    }


    public void SetupComplete(RDFStore store)
    {
      foreach (var item in Definitions)
      {
        DefineRdfsClass(item.Key, store);

        foreach (var word in item.Value)
        {
          RegisterRdfData(item.Key, word, store);
        }
      }
    }


    static readonly Uri RdfType = UrlConstants.Rdf("type");

    static readonly Uri RdfsLabel = UrlConstants.Rdfs("label");

    static readonly Uri RdfsClass = UrlConstants.Rdfs("Class");


    private void RegisterRdfData(string mainClass, WordDefinition word, RDFStore store)
    {
      // Create subject identifier from word
      string propId = StringUtility.Word2Identifier(word.Word);
      Uri subject = UrlConstants.ResourceUrl(propId);

      // Define word as of type mainClass
      Uri type = UrlConstants.ResourceUrl(mainClass);
      store.Insert(NodeFactory.CreateUriNode(subject), NodeFactory.CreateUriNode(RdfType), NodeFactory.CreateUriNode(type));

      // Define word as label for itself
      store.Insert(NodeFactory.CreateUriNode(subject), NodeFactory.CreateUriNode(RdfsLabel), word.Word.ToLiteral(NodeFactory));

      // Define all properties associated with word
      foreach (var prop in word.RdfDefinitions)
      {
        propId = StringUtility.Word2Identifier(prop.Name);
        Uri propUri = UrlConstants.PropertyUrl(propId);

        foreach (var value in prop.Values)
        {
          store.Insert(NodeFactory.CreateUriNode(subject), NodeFactory.CreateUriNode(propUri), value.BuildRdfNode(NodeFactory));
        }
      }
    }


    private void DefineRdfsClass(string c, RDFStore store)
    {
      Uri subject = UrlConstants.ResourceUrl(c);
      store.Update(NodeFactory.CreateUriNode(subject), NodeFactory.CreateUriNode(RdfType), NodeFactory.CreateUriNode(RdfsClass));
      store.Update(NodeFactory.CreateUriNode(subject), NodeFactory.CreateUriNode(RdfsLabel), c.ToLiteral(NodeFactory));
    }
  }
}
