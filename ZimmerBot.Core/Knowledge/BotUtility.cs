#define USERANDOMREACTION

using System.Collections.Generic;
using log4net;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    static ILog DiaLogger = LogManager.GetLogger("DialogLogger");


    public static Response Invoke(KnowledgeBase kb, Request req, bool executeScheduledRules = false)
    {
      RequestState state = new RequestState();

      // Add session store
      Session session = SessionManager.GetOrCreateSession(req.SessionId);
      state[Constants.SessionStoreKey] = session.Store;

      // Add user store
      var userStore = new RDFDictionaryWrapper(kb.MemoryStore, AppSettings.RDF_BaseUrl + "users/" + req.UserId, AppSettings.RDF_BaseUrl + "uservalues/");
      state[Constants.UserStoreKey] = userStore;

      // Add bot store
      var botStore = new RDFDictionaryWrapper(kb.MemoryStore, AppSettings.RDF_BaseUrl + "bot/" + req.UserId, AppSettings.RDF_BaseUrl + "botvalues/");
      state[Constants.BotStoreKey] = botStore;

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
          var pipelineItem = new InputPipelineItem(kb, state, req, input);
          kb.InputPipeline.Invoke(pipelineItem);
          output.AddRange(pipelineItem.Output);
        }
      }
      else
      {
        DiaLogger.InfoFormat("Invoke without input");
        var pipelineItem = new InputPipelineItem(kb, state, req, null);
        kb.InputPipeline.Invoke(pipelineItem);
        output.AddRange(pipelineItem.Output);
      }

      if (output.Count > 0)
      {
        state[Constants.SessionStoreKey][Constants.ResponseCountKey] = state[Constants.SessionStoreKey][Constants.ResponseCountKey] + 1;

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
