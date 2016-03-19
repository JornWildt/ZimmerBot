using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Id { get; protected set; }

    public Domain Domain { get; set; }

    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }

    protected double? ScoreModifier { get; set; }

    protected CallBinding ResponseBinding { get; set; }

    protected List<Func<Domain,Rule>> ExpectedAnswers { get; set; }

    protected int? TimeToLive { get; set; }


    public Rule(Domain d, params object[] topics)
    {
      Condition.Requires(d, nameof(d)).IsNotNull();

      Id = Guid.NewGuid().ToString();
      Domain = d;
      Trigger = new Trigger(topics);
      ExpectedAnswers = new List<Func<Domain,Rule>>();
    }


    public Rule Describe(string description)
    {
      Description = description;
      return this;
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


    public Rule WithScoreModifier(double m)
    {
      ScoreModifier = m;
      return this;
    }


    public Rule WithResponse(CallBinding b)
    {
      b.VerifyBinding();
      ResponseBinding = b;
      return this;
    }


    public Rule WithResponse(string s)
    {
      ProcessorRegistration p = new ProcessorRegistration("echo", inp => TextMerge.MergeTemplate(s, inp.Context.Match.Matches));
      return WithResponse(new CallBinding(p));
    }


    public Rule ExpectAnswer(Func<Domain,Rule> answerSetup)
    {
      ExpectedAnswers.Add(answerSetup);
      return this;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      Trigger.RegisterScheduledJobs(scheduler, botId, Id);
    }


    public Reaction CalculateReaction(EvaluationContext context)
    {
      if (TimeToLive != null)
      {
        if (TimeToLive.Value == 0)
        {
          Domain.QueueRuleForRemoval(this);
          return null;
        }
        TimeToLive = TimeToLive.Value - 1;
      }

      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return null;

      WRegex.MatchResult result = Trigger.CalculateTriggerScore(context);

      if (ScoreModifier != null)
        result.Score = result.Score * ScoreModifier.Value;

      if (result.Score < 0.5)
        return null;

      ResponseContext rc = new ResponseContext(context.State, context.Input, result);
      return new Reaction(rc, this);
    }


    public string Invoke(ResponseContext context)
    {
      string result = ResponseBinding.Invoke(context);

      foreach (var ea in ExpectedAnswers)
      {
        Rule r = ea(Domain);
        r.TimeToLive = 1;
      }

      return result;
    }
  }
}
