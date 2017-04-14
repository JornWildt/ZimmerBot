using System.Collections.Generic;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class TopicRule : Rule
  {
    public TopicRule(KnowledgeBase kb, string label, Topic topic, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight)
    {
      List<Reaction> reactions = new List<Reaction>();
      MatchResult result = new MatchResult(1.0 * weight);
      reactions.Add(new Reaction(new ResponseGenerationContext(context.InputContext, result), this, null));
      return reactions;
    }
  }
}
