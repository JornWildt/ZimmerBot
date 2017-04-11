using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public class Topic
  {
    public string Name { get; protected set; }

    public IList<Rule> StandardRules { get; protected set; }

    public IList<TopicRule> TopicRules { get; protected set; }

    public IEnumerable<RuleBase> AllRules { get { return StandardRules.Cast<RuleBase>().Concat(TopicRules.Cast<RuleBase>()); } }


    public Topic(string name)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty(); ;

      Name = name;
      StandardRules = new List<Rule>();
      TopicRules = new List<TopicRule>();
    }


    public void AddRule(RuleBase r)
    {
      if (r is Rule)
        StandardRules.Add((Rule)r);
      else
        TopicRules.Add((TopicRule)r);
    }
  }
}
