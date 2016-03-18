using System;
using Quartz;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;
using static ZimmerBot.Core.Knowledge.ProcessorRegistry;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Id { get; protected set; }

    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }

    protected double? ScoreModifier { get; set; }

    protected Invocation OutputGenerator { get; set; } // FIXME: better naming, cleanup


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


    public Rule Response(Invocation f)
    {
      OutputGenerator = f;
      return this;
    }


    public Rule Response(string s)
    {
      ProcessorRegistration p = new ProcessorRegistration("echo", inp => () => TextMerge.MergeTemplate(s, inp.Context.Match.Matches));
      OutputGenerator = new Invocation(p);
      return this;
      //return Response(c => new Invocation(null, () => TextMerge.MergeTemplate(s, c.Match.Matches)));
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

      ResponseContext rc = new ResponseContext(context.State, context.Input, result);
      return new Reaction(rc, OutputGenerator);
    }
  }
}
