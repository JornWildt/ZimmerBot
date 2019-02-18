using System;
using System.Linq;
using System.Collections.Generic;
using VDS.RDF;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ChatLoggerStage : IPipelineHandler<InputPipelineItem>
  {
    static INodeFactory NodeFactory = new NodeFactory();


    public void Handle(InputPipelineItem item)
    {
      // Protect against recursive invokes from templates - only register initial invoke
      if (!item.Context.FromTemplate && AppSettings.RDF_EnableChatLog)
      {
        AddEntry(item, item.Context.Request.Input, UrlConstants.UsersUrl(item.Context.Request.UserId));

        if (item.Output.Count > 0)
        {
          string output = item.Output.Aggregate((a, b) => a + "\n" + b);
          AddEntry(item, output, UrlConstants.BotUrl);
        }
      }
    }

    protected void AddEntry(InputPipelineItem item, string text, Uri creator)
    {
      if (text != null && text.Length > 200)
        text = text.Substring(0, 200);

      INode s = NodeFactory.CreateUriNode(UrlConstants.ChatEntriesUrl(Guid.NewGuid().ToString()));

      INode p = NodeFactory.CreateUriNode(UrlConstants.Rdf("type"));
      INode o = NodeFactory.CreateUriNode(UrlConstants.ChatEntryTypeUrl);
      item.Context.KnowledgeBase.MemoryStore.Insert(s, p, o, RDFStore.DynamicStoreName);

      p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("created"));
      o = DateTime.Now.ToLiteral(NodeFactory);
      item.Context.KnowledgeBase.MemoryStore.Insert(s, p, o, RDFStore.DynamicStoreName);

      p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("creator"));
      o = NodeFactory.CreateUriNode(creator);
      item.Context.KnowledgeBase.MemoryStore.Insert(s, p, o, RDFStore.DynamicStoreName);

      p = NodeFactory.CreateUriNode(UrlConstants.ChatReferenceTypeUrl);
      o = NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(item.Context.Session.SessionId));
      item.Context.KnowledgeBase.MemoryStore.Insert(s, p, o, RDFStore.DynamicStoreName);

      if (text != null)
      {
        p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("description"));
        o = text.ToLiteral(NodeFactory);
        item.Context.KnowledgeBase.MemoryStore.Insert(s, p, o, RDFStore.DynamicStoreName);
      }
    }
  }
}
