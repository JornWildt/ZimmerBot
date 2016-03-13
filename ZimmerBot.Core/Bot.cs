using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core
{
  public class Bot
  {
    protected KnowledgeBase KnowledgeBase { get; set; }

    protected BotState State { get; set; }


    public Bot(KnowledgeBase kb)
    {
      KnowledgeBase = kb;
      State = new BotState();

      State["dialogue.responseCount"] = 0;
    }


    public Response Invoke(Request req)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZTokenString input = tokenizer.Tokenize(req.Input);

      KnowledgeBase.ExpandTokens(input);
      EvaluationContext context = new EvaluationContext(State, input);
      ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

      string[] output;

      if (reactions.Count > 0)
        output = reactions.Select(r => r.GenerateResponse(input)).ToArray();
      else
        output = new string[] { "???" };

      State["dialogue.responseCount"] = (int)State["dialogue.responseCount"] + 1;

      return new Response
      {
        Output = output
      };
    }
  }
}
