using System.Collections.Generic;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class TopicRule : Rule
  {
    public TopicRule(KnowledgeBase kb, string label, Topic topic, List<Statement> statements)
      : base(kb, label, topic, null, statements)
    {
    }


    public override Executable WithCondition(Expression c)
    {
      // Hmmm ... maybe conditions should be allowed on topics too?
      return this;
    }


    public override Executable WithWeight(double w)
    {
      return this;
    }


    public override void SetupComplete()
    {
      // Do nothing
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight)
    {
      List<Reaction> reactions = new List<Reaction>();
      MatchResult result = new MatchResult(1.0 * weight);
      reactions.Add(new Reaction(new ResponseGenerationContext(context.InputContext, result), this, null));
      return reactions;
    }


    public override List<string> Invoke(ResponseGenerationContext context, string outputId)
    {
      // Prepare for next topic rule whenever a topic rule is invoked
      context.Session.IncrementTopicRuleIndex(RelatedTopic.Name);

      return base.Invoke(context, outputId);
    }

    public override string ToString()
    {
      return "Topic rule";
    }
  }
}
