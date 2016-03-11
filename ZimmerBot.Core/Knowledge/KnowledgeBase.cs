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


    public void ExpandTokens(TokenString input)
    {
      foreach (Domain d in Domains)
      {
        d.ExpandTokens(input);
      }
    }


    public ReactionSet FindMatchingReactions(TokenString input)
    {
      ReactionSet reactions = new ReactionSet();

      foreach (Domain d in Domains)
        d.FindMatchingReactions(input, reactions);

      return reactions;
    }
  }
}
