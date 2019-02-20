using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public abstract class Rule : Executable
  {
    public string Id { get; protected set; }

    public string Label { get; protected set; }

    public Topic RelatedTopic { get; protected set; }

    public string StartingTopic { get; protected set; }


    public Rule(KnowledgeBase kb, string label, Topic topic, IEnumerable<RuleModifier> modifiers, IList<Statement> statements)
      : base(kb, modifiers, statements)
    {
      Id = Guid.NewGuid().ToString();
      Label = label;
      RelatedTopic = topic;

      if (label != null)
        KnowledgeBase.RegisterRuleLabel(label, this);

      StatementInitializationContext context = new StatementInitializationContext(this);
      foreach (Statement o in Statements)
      {
        o.Initialize(context);
        if (o is StartTopicStatement)
          StartingTopic = ((StartTopicStatement)o).Topic;
      }
    }


    public override string ToString()
    {
      return string.IsNullOrEmpty(Label) ? "" : "<" + Label + ">. Statements: #" + Statements.Count;
    }


    public virtual void RegisterParentRule(Rule parentRule)
    {
    }


    public abstract IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight);


    protected List<Reaction> SelectReactions(TriggerEvaluationContext context, MatchResult result)
    {
      Statement.RepatableMode repeatable = Statements.Max(s => s.Repeatable);
      List<Reaction> reactions = new List<Reaction>();

      if (repeatable != Statement.RepatableMode.AutomaticRepeatable && repeatable != Statement.RepatableMode.ForcedRepeatable)
      {
        // This is not a repeatable rule, so split it into multiple reactions that looks at output usage

        // Add one reaction for each output statement
        reactions = Statements
          .OfType<OutputTemplateStatement>()
          .Select(s => new Reaction(MakeWeightedReaction(context, result, s.Template), this, s.Template.Id))
          .ToList();
      }

      // If no output statements are found then ensure we have at least one reaction (that does not identify an output)
      if (reactions.Count == 0)
        reactions.Add(new Reaction(new ResponseGenerationContext(context.InputContext, result), this, null));

      return reactions
             .Where(s => s.Score > 0.0)
             .ToList();
    }


    protected ResponseGenerationContext MakeWeightedReaction(TriggerEvaluationContext context, MatchResult result, OutputTemplate template)
    {
      int outputUsageCount = context.InputContext.Session.GetUsageCount(template.Id);

      // Reduce the amount of repetition in output by lowering the reaction score by the number of times it has been used
      double score = result.Score * Math.Pow(0.99, outputUsageCount);
      //double score = result.Score * (outputUsageCount > 0 ? 0.0 : 1.0);

      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(score, result.Matches));
      return rc;
    }
  }
}
