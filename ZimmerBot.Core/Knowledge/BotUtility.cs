#define USERANDOMREACTION

using System;
using System.Collections.Generic;
using log4net;
using VDS.RDF;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    static ILog DiaLogger = LogManager.GetLogger("DialogLogger");

    static INodeFactory NodeFactory = new NodeFactory();


    public static Response Invoke(KnowledgeBase kb, Request req, bool executeScheduledRules = false)
    {
      return InvokeInternal(kb, req, executeScheduledRules, false);
    }


    internal static Response InvokeInternal(KnowledgeBase kb, Request req, bool executeScheduledRules, bool fromTemplate)
    {
      RequestState state = new RequestState();

      // Add session store
      Session session = SessionManager.GetOrCreateSession(req.SessionId);
      state[StateKeys.SessionStore] = session.Store;

      // Add user store
      var userStore = new RDFDictionaryWrapper(kb.MemoryStore, UrlConstants.UsersUrl(req.UserId), UrlConstants.UserValuesUrl);
      state[StateKeys.UserStore] = userStore;

      // Add bot store
      var botStore = new RDFDictionaryWrapper(kb.MemoryStore, UrlConstants.BotUrl, UrlConstants.BotValuesUrl);
      state[StateKeys.BotStore] = botStore;

      // Register <user, is-a, user>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.UsersUrl(req.UserId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.UserTypeUrl));

      // Register <chat, is-a, chat>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(req.SessionId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.ChatTypeUrl));

      List<string> output = new List<string>();

      if (req.Input != null)
      {
        DiaLogger.InfoFormat("Invoke: {0}", req.Input);
        ZTokenizer tokenizer = new ZTokenizer();
        ZStatementSequence statements = tokenizer.Tokenize(req.Input);

        // Always evaluate at least one empty statement in order to invoke triggers without regex
        if (statements.Statements.Count == 0)
          statements.Statements.Add(new ZTokenSequence());

        foreach (ZTokenSequence input in statements.Statements)
        {
          var pipelineItem = new InputPipelineItem(kb, session, state, req, input, fromTemplate);
          kb.InputPipeline.Invoke(pipelineItem);
          output.AddRange(pipelineItem.Output);
        }
      }
      else
      {
        DiaLogger.InfoFormat("Invoke without input");
        var pipelineItem = new InputPipelineItem(kb, session, state, req, null, fromTemplate);
        kb.InputPipeline.Invoke(pipelineItem);
        output.AddRange(pipelineItem.Output);
      }

      if (output.Count > 0)
      {
        state[StateKeys.SessionStore][StateKeys.ResponseCount] = state[StateKeys.SessionStore][StateKeys.ResponseCount] + 1;

        return new Response
        {
          Output = output.ToArray(),
          State = req.State
        };
      }
      else
      {
        return new Response { Output = new string[0] };
      }
    }
  }
}
