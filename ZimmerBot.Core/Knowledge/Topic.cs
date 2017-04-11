using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public class Topic
  {
    public string Name { get; protected set; }

    public IList<StandardRule> StandardRules { get; protected set; }

    public IList<TopicRule> TopicRules { get; protected set; }

    public IEnumerable<Rule> AllRules { get { return StandardRules.Cast<Rule>().Concat(TopicRules.Cast<Rule>()); } }


    public Topic(string name)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty(); ;

      Name = name;
      StandardRules = new List<StandardRule>();
      TopicRules = new List<TopicRule>();
    }


    public void AddRule(Rule r)
    {
      if (r is StandardRule)
        StandardRules.Add((StandardRule)r);
      else
        TopicRules.Add((TopicRule)r);
    }
  }
}
