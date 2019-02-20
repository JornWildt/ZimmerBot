using System.Collections.Generic;
using Quartz;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class StandardRule : Rule
  {
    public Trigger Trigger { get; protected set; }

    public double? Weight { get; protected set; }


    public StandardRule(KnowledgeBase kb, string label, Topic topic, List<WRegexBase> patterns, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
      if (patterns == null || patterns.Count == 0 || patterns[0] == null)
        Trigger = new EmptyTrigger();
      else
        Trigger = new RegexTrigger(patterns);
      HandleModifiers(modifiers);
    }


    public StandardRule(KnowledgeBase kb, string label, Topic topic, List<OperatorKeyValueList> patterns, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
      Trigger = new FuzzyTrigger(patterns);
      HandleModifiers(modifiers);
    }


    public StandardRule(KnowledgeBase kb, List<Statement> statements)
      : base(kb, null, null, statements)
    {
    }


    private void HandleModifiers(List<RuleModifier> modifiers)
    {
      if (modifiers != null)
        foreach (var m in modifiers)
          m.Invoke(this);
    }


    public override string ToString()
    {
      string t = Trigger != null ? Trigger.ToString() : "<empty>";
      return $"{t} (w: {Weight}) => " + base.ToString();
    }


    public StandardRule WithCondition(Expression c)
    {
      Trigger.WithCondition(c);
      return this;
    }


    public StandardRule WithWeight(double w)
    {
      Weight = w;
      return this;
    }


    public override void RegisterParentRule(Rule parentRule)
    {
      Trigger.RegisterParentRule(parentRule);
    }


    public void RegisterTopic(Topic t)
    {
      RelatedTopic = t;
    }


    public override void SetupComplete()
    {
      if (SpellChecker.IsInitialized)
        Trigger.ExtractWordsForSpellChecker();
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight)
    {
      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return new List<Reaction>();
      if (context.RestrictToRuleLabel != null && context.RestrictToRuleLabel != Label)
        return new List<Reaction>();

      context.StartingTopic = StartingTopic;

      MatchResult result = Trigger.CalculateTriggerScore(context);

      if (Weight != null)
        result.Score = result.Score * Weight.Value;

      // Adjust with external weight
      result.Score = result.Score * weight;

      List<Reaction> reactions = SelectReactions(context, result);
      return reactions;
    }
  }
}
