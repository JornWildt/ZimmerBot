using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class PatternRule : Rule
  {
    public StringPairList Pattern { get; protected set; }


    public PatternRule(KnowledgeBase kb, string label, Topic topic, StringPairList pattern, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
      Condition.Requires(pattern, nameof(pattern)).IsNotNull();

      Pattern = pattern;
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight)
    {
      return new List<Reaction>();
    }


    public override string ToString()
    {
      return Pattern.ToString();
    }
  }
}
