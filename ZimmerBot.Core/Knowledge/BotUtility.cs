using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    static ILog DiaLogger = LogManager.GetLogger("DialogLogger");


    public static Response Invoke(KnowledgeBase kb, BotState state, Request req, bool executeScheduledRules = false)
    {
      List<string> output = new List<string>();
      ReactionSet reactions = null;

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
          kb.ExpandTokens(input);
          EvaluationContext context = new EvaluationContext(state, input, req.RuleId, executeScheduledRules);

          // FIXME: only use the last statement? Makes some sense though, but maybe all would be better?
          reactions = kb.FindMatchingReactions(context);
        }
      }
      else
      {
        DiaLogger.InfoFormat("Invoke without input");
        EvaluationContext context = new EvaluationContext(state, null, req.RuleId, executeScheduledRules);
        reactions = kb.FindMatchingReactions(context);
      }

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

      if (output.Count > 0)
      {
        state["state.conversation.entries.Count"] = (double)state["state.conversation.entries.Count"] + 1;

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
