using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core
{
  public class Bot
  {
    protected KnowledgeBase KnowledgeBase { get; set; }


    public Bot(KnowledgeBase kb)
    {
      KnowledgeBase = kb;
    }


    public Response Invoke(Request req)
    {
      Tokenizer tokenizer = new Tokenizer();
      TokenString input = tokenizer.Tokenize(req.Input);

      KnowledgeBase.ExpandTokens(input);
      ReactionSet reactions = KnowledgeBase.FindMatchingReactions(input);

      string[] output;

      if (reactions.Count > 0)
        output = reactions.Select(r => r.GenerateResponse(input)).ToArray();
      else
        output = new string[] { "???" };

      return new Response
      {
        Output = output
      };
    }
  }
}
