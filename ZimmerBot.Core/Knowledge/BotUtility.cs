#define USERANDOMREACTION

using System;
using System.Collections.Generic;
using log4net;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    static ILog DiaLogger = LogManager.GetLogger("DialogLogger");

    static Random Randomizer = new Random();


    public static Response Invoke(KnowledgeBase kb, Request req, bool executeScheduledRules = false)
    {
      RequestState state = new RequestState();

      Session session = SessionManager.GetOrCreateSession(req.SessionId);
      state[Constants.SessionStoreKey] = session.Store;

      var userStore = new RDFDictionaryWrapper(kb.MemoryStore, AppSettings.RDF_BaseUrl + "users/" + req.UserId, AppSettings.RDF_BaseUrl + "uservalues/");
      state[Constants.UserStoreKey] = userStore;

      var botStore = new RDFDictionaryWrapper(kb.MemoryStore, AppSettings.RDF_BaseUrl + "bot/" + req.UserId, AppSettings.RDF_BaseUrl + "botvalues/");
      state[Constants.BotStoreKey] = botStore;

      List<ReactionSet> reactionList = new List<ReactionSet>();

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
          reactionList.Add(pipelineItem.Reactions);
        }
      }
      else
      {
        DiaLogger.InfoFormat("Invoke without input");
        var pipelineItem = new InputPipelineItem(kb, state, req, null);
        kb.InputPipeline.Invoke(pipelineItem);
        reactionList.Add(pipelineItem.Reactions);
      }

      List<string> output = new List<string>();

      foreach (var reactions in reactionList)
      {
        if (reactions != null && reactions.Count > 0)
        {
#if USERANDOMREACTION
          // Select a random reaction
          Reaction r = reactions[Randomizer.Next(reactions.Count)];
#else
          // Use all reactions
          foreach (Reaction r in reactions)
          {
#endif
            string response = r.GenerateResponse();
            DiaLogger.InfoFormat("Response: " + response);

            foreach (string line in response.Replace("\r", "").Split('\n'))
              output.Add(line);

            state[Constants.SessionStoreKey][Constants.LastRuleIdKey] = r.Rule.Id;
#if !USERANDOMREACTION
          }
#endif
        }
        else
        {
          DiaLogger.InfoFormat("No suitable response found");
          if (req.Input != null)
            output.Add("???");
        }
      }

      if (output.Count > 0)
      {
        state[Constants.SessionStoreKey][Constants.LineCountKey] = state[Constants.SessionStoreKey][Constants.LineCountKey] + 1;

        Response response = new Response
        {
          Output = output.ToArray(),
          State = req.State
        };

        return response;
      }
      else
      {
        return new Response { Output = new string[0] };
      }
    }
  }
}
