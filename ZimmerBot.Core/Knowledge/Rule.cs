using System;
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

    protected double? ScoreModifier { get; set; }


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


    public Rule WithScoreModifier(double m)
    {
      ScoreModifier = m;
      return this;
    }


    public Rule Response(Func<WRegex.MatchResult, Func<string>> g)
    {
      OutputGenerator = g;
      return this;
    }


    public Rule Response(string s)
    {
      return Response(i => () => TextMerge.MergeTemplate(s, i.Matches));
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      Trigger.RegisterScheduledJobs(scheduler, botId, Id);
    }


    public Reaction CalculateReaction(EvaluationContext context)
    {
      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return null;

      WRegex.MatchResult result = Trigger.CalculateTriggerScore(context);

      if (ScoreModifier != null)
        result.Score = result.Score * ScoreModifier.Value;

      if (result.Score < 0.5)
        return null;

      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();

      if (context.Input != null)
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
