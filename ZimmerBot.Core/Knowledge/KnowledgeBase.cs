using System.Collections.Generic;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class KnowledgeBase
  {
    protected List<Domain> Domains { get; set; }


    public KnowledgeBase()
    {
      Domains = new List<Domain>();
    }


    public Domain NewDomain(string name)
    {
      Domain d = new Domain(name);
      Domains.Add(d);
      return d;
    }


    public void ExpandTokens(TokenString tokens)
    {
      foreach (Domain d in Domains)
      {
        d.ExpandTokens(tokens);
      }
    }


    public IList<Reaction> FindMatchingReactions(TokenString tokens)
    {
      IList<Reaction> reactions = new List<Reaction>();

      foreach (Domain d in Domains)
        d.FindMatchingReactions(tokens, reactions);

      return reactions;
    }
  }
}
