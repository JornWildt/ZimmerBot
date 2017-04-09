using System.Collections.Generic;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public class Topic
  {
    public string Name { get; protected set; }

    public IList<string> TriggerWords { get; protected set; }

    public IList<Rule> Rules { get; protected set; }

    public Topic(string name, IList<string> triggerWords, IList<Rule> rules)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrEmpty(); ;
      Condition.Requires(triggerWords, nameof(triggerWords)).IsNotNull();
      Condition.Requires(rules, nameof(rules)).IsNotNull();

      Name = name;
      TriggerWords = triggerWords;
      Rules = rules;
    }
  }
}
