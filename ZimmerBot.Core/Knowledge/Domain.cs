using System.Collections.Generic;
using CuttingEdge.Conditions;
using Quartz;
using ZimmerBot.Core.Parser;


namespace ZimmerBot.Core.Knowledge
{
  public class Domain
  {
    public string Name { get; protected set; }

    public List<Concept> Concepts { get; protected set; }

    public List<Rule> Rules { get; protected set; }

    public KnowledgeBase KnowledgeBase { get; protected set; }


    internal Domain(KnowledgeBase kb, string name)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(name, nameof(name)).IsNotNull();

      KnowledgeBase = kb;
      Concepts = new List<Concept>();
      Rules = new List<Rule>();
      Name = name;
    }


    public Concept AddConcept(string name, IEnumerable<string> words)
    {
      Concept w = new Concept(name, words);
      Concepts.Add(w);
      return w;
    }


    public Rule AddRule(params object[] topics)
    {
      Rule r = new Rule(this, topics);
      Rules.Add(r);
      return r;
    }


    public void RemoveRule(Rule r)
    {
      Rules.Remove(r);
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      foreach (Rule r in Rules)
      {
        r.RegisterScheduledJobs(scheduler, botId);
      }
    }


    public void ExpandTokens(ZTokenSequence input)
    {
      foreach (ZToken t in input)
      {
        foreach (Concept w in Concepts)
        {
          w.ExpandToken(t);
        }
      }
    }


    public void FindMatchingReactions(EvaluationContext context, ReactionSet reactions)
    {
      List<Rule> safeRuleList = new List<Rule>(Rules);

      foreach (Rule r in safeRuleList)
      {
        Reaction reaction = r.CalculateReaction(context);
        if (reaction != null)
          reactions.Add(reaction);
      }
    }
  }
}
