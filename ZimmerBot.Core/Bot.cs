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
      IList<Reaction> reactions = KnowledgeBase.FindMatchingReactions(input);

      string output = "Duh!";

      if (reactions.Count > 0)
        output = reactions.Select(r => r.GenerateResponse(input)).Aggregate((a, b) => a + "\n" + b);
      else
        output = input.Select(t => t.OriginalText).Aggregate((a, b) => a + "." + b);

      return new Response
      {
        Output = output
      };
    }
  }
}
