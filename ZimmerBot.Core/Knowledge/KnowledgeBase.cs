using System.Collections.Generic;
using ZimmerBot.Core.Parser;


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


    public IEnumerable<Domain> GetDomains()
    {
      return Domains;
    }


    public void ExpandTokens(ZTokenSequence input)
    {
      foreach (Domain d in Domains)
      {
        d.ExpandTokens(input);
      }
    }


    public ReactionSet FindMatchingReactions(EvaluationContext context)
    {
      ReactionSet reactions = new ReactionSet();

      foreach (Domain d in Domains)
        d.FindMatchingReactions(context, reactions);

      return reactions;
    }
  }
}
