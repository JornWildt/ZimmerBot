using System;
using System.Linq;
using System.Collections.Generic;
using VDS.RDF;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ChatLoggerStage : IPipelineHandler<InputPipelineItem>
  {
    static INodeFactory NodeFactory = new NodeFactory();


    public void Handle(InputPipelineItem item)
    {
      // Protect against recursive invokes from templates - only register initial invoke
      if (!item.FromTemplate && AppSettings.RDF_EnableChatLog)
      {
        AddEntry(item, item.Request.Input, UrlConstants.UsersUrl(item.Request.UserId));

        if (item.Output.Count > 0)
        {
          string output = item.Output.Aggregate((a, b) => a + "\n" + b);
          AddEntry(item, output, UrlConstants.BotUrl);
        }
      }
    }

    protected void AddEntry(InputPipelineItem item, string text, Uri creator)
    {
      INode s = NodeFactory.CreateUriNode(UrlConstants.ChatEntriesUrl(Guid.NewGuid().ToString()));

      INode p = NodeFactory.CreateUriNode(UrlConstants.Rdf("type"));
      INode o = NodeFactory.CreateUriNode(UrlConstants.ChatEntryTypeUrl);
      item.KnowledgeBase.MemoryStore.Insert(s, p, o);

      p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("created"));
      o = DateTime.Now.ToLiteral(NodeFactory);
      item.KnowledgeBase.MemoryStore.Insert(s, p, o);

      p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("creator"));
      o = NodeFactory.CreateUriNode(creator);
      item.KnowledgeBase.MemoryStore.Insert(s, p, o);

      p = NodeFactory.CreateUriNode(new Uri("http://chat"));
      o = NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(item.Session.SessionId));
      item.KnowledgeBase.MemoryStore.Insert(s, p, o);

      if (text != null)
      {
        p = NodeFactory.CreateUriNode(UrlConstants.DcTerms("description"));
        o = text.ToLiteral(NodeFactory);
        item.KnowledgeBase.MemoryStore.Insert(s, p, o);
      }

      //foreach (string output in item.Output)
      //{
      //  // Register at most 200 characters
      //  string so = output.Substring(0, Math.Min(output.Length, 200));

      //  p = NodeFactory.CreateUriNode(new Uri("http://output"));
      //  o = so.ToLiteral(NodeFactory);
      //  item.KnowledgeBase.MemoryStore.Insert(s, p, o);
      //}
    }
  }
}
