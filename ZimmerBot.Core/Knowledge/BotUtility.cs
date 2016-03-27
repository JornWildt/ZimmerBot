using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    public static Response Invoke(KnowledgeBase kb, BotState state, Request req, bool executeScheduledRules = false)
    {
      List<string> output = new List<string>();

      if (req.Input != null)
      {
        ZTokenizer tokenizer = new ZTokenizer();
        ZStatementSequence statements = tokenizer.Tokenize(req.Input);

        // Always evaluate at least one empty statement in order to invoke triggers without regex
        if (statements.Statements.Count == 0)
          statements.Statements.Add(new ZTokenSequence());

        foreach (ZTokenSequence input in statements.Statements)
        {
          kb.ExpandTokens(input);
          EvaluationContext context = new EvaluationContext(state, input, req.RuleId, executeScheduledRules);
          ReactionSet reactions = kb.FindMatchingReactions(context);

          if (reactions.Count > 0)
          {
            foreach (Reaction r in reactions)
            {
              string response = r.GenerateResponse();
              foreach (string line in response.Split('\n'))
                output.Add(line);
            }
          }
          else
            output.Add("???");
        }
      }
      else
      {
        EvaluationContext context = new EvaluationContext(state, null, req.RuleId, executeScheduledRules);
        ReactionSet reactions = kb.FindMatchingReactions(context);

        if (reactions.Count > 0)
          foreach (Reaction r in reactions)
            output.Add(r.GenerateResponse());
      }

      if (output.Count > 0)
      {
        state["state.conversation.entries.Count"] = (double)state["state.conversation.entries.Count"] + 1;

        Response response = new Response
        {
          Output = output.ToArray()
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
