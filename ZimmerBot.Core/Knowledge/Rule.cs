﻿using System;
using System.Collections.Generic;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Id { get; protected set; }

    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }


    protected HashSet<string> ParameterMap = new HashSet<string>();


    protected Func<WRegex.MatchResult, Func<string>> OutputGenerator { get; set; } // FIXME: better naming, cleanup


    public Rule(params object[] topics)
    {
      Id = Guid.NewGuid().ToString();
      Trigger = new Trigger(topics);
    }


    public Rule Describe(string description)
    {
      Description = description;
      return this;
    }


    public Rule Parameter(string name)
    {
      ParameterMap.Add(name);
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


    public Rule Response(Func<WRegex.MatchResult, Func<string>> g)
    {
      OutputGenerator = g;
      RuleRepository.Add(this);
      return this;
    }


    public Rule Response(string s)
    {
      return Response(i => () => TextMerge.MergeTemplate(s, i.Matches));
    }


    public void RegisterScheduledJobs(IScheduler scheduler)
    {
      Trigger.RegisterScheduledJobs(scheduler, Id);
    }


    public Reaction CalculateReaction(EvaluationContext context)
    {
      WRegex.MatchResult result = Trigger.CalculateTriggerScore(context);

      if (result.Score < 0.5)
        return null;

      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();

      foreach (ZToken t in context.Input)
        t.ExtractParameter(ParameterMap, generatorParameters);

      // Some parameter values are missing => ignore
      // FIXME: need only counting!
      if (ParameterMap.Count > generatorParameters.Count)
        return null;

      return new Reaction(result.Score, OutputGenerator(result));
    }
  }
}
