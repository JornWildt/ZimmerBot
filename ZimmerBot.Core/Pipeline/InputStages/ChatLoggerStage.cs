using System;
using VDS.RDF;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ChatLoggerStage : IPipelineHandler<InputPipelineItem>
  {
    static INodeFactory NodeFactory = new NodeFactory();


    public void Handle(InputPipelineItem item)
    {
      // Protect against recursive invokes from templates - only register initial invoke
      if (!item.FromTemplate)
      {
        INode s = NodeFactory.CreateUriNode(UrlConstants.ChatEntriesUrl(Guid.NewGuid().ToString()));

        INode p = NodeFactory.CreateUriNode(new Uri("http://timestamp"));
        INode o = DateTime.Now.ToLiteral(NodeFactory);
        item.KnowledgeBase.MemoryStore.Insert(s, p, o);

        p = NodeFactory.CreateUriNode(new Uri("http://participant"));
        o = NodeFactory.CreateUriNode(UrlConstants.UsersUrl(item.Request.UserId));
        item.KnowledgeBase.MemoryStore.Insert(s, p, o);

        p = NodeFactory.CreateUriNode(new Uri("http://chat"));
        o = NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(item.Session.SessionId));
        item.KnowledgeBase.MemoryStore.Insert(s, p, o);

        if (item.Request.Input != null)
        {
          p = NodeFactory.CreateUriNode(new Uri("http://input"));
          o = item.Request.Input.ToLiteral(NodeFactory);
          item.KnowledgeBase.MemoryStore.Insert(s, p, o);
        }

        foreach (string output in item.Output)
        {
          // Register at most 200 characters
          string so = output.Substring(0, Math.Min(output.Length, 200));

          p = NodeFactory.CreateUriNode(new Uri("http://output"));
          o = so.ToLiteral(NodeFactory);
          item.KnowledgeBase.MemoryStore.Insert(s, p, o);
        }

        p = NodeFactory.CreateUriNode(UrlConstants.Rdf("type"));
        o = NodeFactory.CreateUriNode(UrlConstants.ChatEntryTypeUrl);
        item.KnowledgeBase.MemoryStore.Insert(s, p, o);
      }
    }
  }
}
