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
      Session session = SessionManager.GetOrCreateSession(req.SessionId);
      SessionState state = session.State;

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
          foreach (Reaction r in reactions)
          {
            string response = r.GenerateResponse();
            DiaLogger.InfoFormat("Response: " + response);

            foreach (string line in response.Replace("\r", "").Split('\n'))
              output.Add(line);
          }
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
        state[Constants.SessionStore][Constants.LineCount] = state[Constants.SessionStore][Constants.LineCount] + 1;

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
