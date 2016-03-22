﻿using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using Quartz;
using ZimmerBot.Core.ConfigParser;
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

    public Trigger Trigger { get; protected set; }

    public double? ScoreModifier { get; protected set; }

    public List<OutputStatement> OutputStatements { get; protected set; }

    public List<Func<Domain,Rule>> ExpectedAnswers { get; protected set; }

    public int? TimeToLive { get; protected set; }


    #region OBSOLETE

    public List<string> OutputTemplates { get; protected set; }

    public CallBinding ResponseBinding { get; protected set; }

    #endregion


    public Rule(Domain d, params object[] topics)
    {
      Condition.Requires(d, nameof(d)).IsNotNull();

      Id = Guid.NewGuid().ToString();
      Domain = d;
      Trigger = new Trigger(topics);
      OutputTemplates = new List<string>();
      ExpectedAnswers = new List<Func<Domain,Rule>>();

      // Defauly response binding is print out one of the output templates
      ResponseBinding = ResponseHelper.OneOf(OutputTemplates);
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


    public Rule WithOutputStatements(IEnumerable<OutputStatement> output)
    {
      OutputStatements = new List<OutputStatement>(output);
      return this;
    }


    #region OBSOLETE

    public Rule WithResponse(CallBinding b)
    {
      b.VerifyBinding();
      ResponseBinding = b;
      return this;
    }


    public Rule WithResponse(string s)
    {
      OutputTemplates.Add(s);
      return this;
    }


    public Rule WithResponses(IEnumerable<string> s)
    {
      OutputTemplates.AddRange(s);
      return this;
    }

    #endregion


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
          Domain.RemoveRule(this);
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


    public static Random Randomizer = new Random();


    public string Invoke(ResponseContext context)
    {
      OutputExecutionContect ox_context = new OutputExecutionContect(context);

      foreach (OutputStatement output in OutputStatements)
      {
        output.Execute(ox_context);
      }

      // FIXME: do some more efficient combination of matches, botstate and other state elements for lookup
      //foreach (var m in context.Match.Matches)
      //{
      //  ec.State[m.Key] = m.Value;
      //  ec.State["$" + m.Key] = m.Value;
      //}

      string selectedTemplate = ox_context.OutputTemplates[Randomizer.Next(ox_context.OutputTemplates.Count)];
      string result = TextMerge.MergeTemplate(selectedTemplate, ox_context.LastValue);


      foreach (var ea in ExpectedAnswers)
      {
        Rule r = ea(Domain);
        r.TimeToLive = 1;
        if (r.ScoreModifier != null)
          r.ScoreModifier *= 2;
        else
          r.ScoreModifier = 2;
      }

      return result;
    }
  }
}
