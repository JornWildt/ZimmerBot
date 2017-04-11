﻿using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Quartz;
using Quartz.Impl;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Scheduler;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule : RuleBase
  {
    public Trigger Trigger { get; protected set; }

    public double? Weight { get; protected set; }



    public Rule(KnowledgeBase kb, string label, Topic topic, WRegexBase pattern, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, label, topic, statements)
    {
      Trigger = new Trigger(pattern);

      if (modifiers != null)
        foreach (var m in modifiers)
          m.Invoke(this);
    }



    public Rule WithCondition(Expression c)
    {
      Trigger.WithCondition(c);
      return this;
    }


    public Rule WithSchedule(TimeSpan interval)
    {
      Trigger.WithSchedule(interval);
      return this;
    }


    public Rule WithWeight(double w)
    {
      Weight = w;
      return this;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      Trigger.RegisterScheduledJobs(scheduler, botId, Id);
    }


    public override void RegisterParentRule(RuleBase parentRule)
    {
      Trigger.RegisterParentRule(parentRule);
    }


    public void RegisterTopic(Topic t)
    {
      RelatedTopic = t;
    }


    public override IList<Reaction> CalculateReactions(TriggerEvaluationContext context)
    {
      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return new List<Reaction>();
      if (context.RestrictToRuleLabel != null && context.RestrictToRuleLabel != Label)
        return new List<Reaction>();

      MatchResult result = Trigger.CalculateTriggerScore(context);

      if (Weight != null)
        result.Score = result.Score * Weight.Value;

      // FIXME: Why exactly 0.5?
      if (result.Score < 0.5)
        return new List<Reaction>();

      List<Reaction> reactions = SelectReactions(context, result);
      return reactions;
    }


    private List<Reaction> SelectReactions(TriggerEvaluationContext context, MatchResult result)
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

      return reactions;
    }


    private ResponseGenerationContext MakeWeightedReaction(TriggerEvaluationContext context, MatchResult result, OutputTemplate template)
    {
      int outputUsageCount = context.InputContext.Session.GetUsageCount(template.Id);

      // Reduce the amount of repetition in output by lowering the reaction score by the number of times it has been used
      double score = result.Score * Math.Pow(0.99, outputUsageCount);

      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(score, result.Matches));
      return rc;
    }
  }
}
