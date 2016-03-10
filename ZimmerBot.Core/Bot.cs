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
      TokenString tokens = tokenizer.Tokenize(req.Input);

      KnowledgeBase.ExpandTokens(tokens);
      IList<Reaction> reactions = KnowledgeBase.FindMatchingReactions(tokens);

      string output = "Duh!";

      if (reactions.Count > 0)
        output = reactions[0].GenerateResponse(tokens);
      else
        output = tokens.Select(t => t.OriginalText).Aggregate((a, b) => a + "." + b);

      return new Response
      {
        Output = output
      };
    }
  }
}
