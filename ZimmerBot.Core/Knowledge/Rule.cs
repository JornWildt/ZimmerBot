using System;
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

    public double? Weight { get; protected set; }

    public List<OutputStatement> OutputStatements { get; protected set; }

    public List<Func<Domain,Rule>> ExpectedAnswers { get; protected set; }

    public int? TimeToLive { get; protected set; }


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


    public Rule WithWeight(double w)
    {
      Weight = w;
      return this;
    }


    public Rule WithOutputStatements(IEnumerable<OutputStatement> output)
    {
      OutputStatements = new List<OutputStatement>(output);
      return this;
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
          Domain.RemoveRule(this);
          return null;
        }
        TimeToLive = TimeToLive.Value - 1;
      }

      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return null;

      WRegex.MatchResult result = Trigger.CalculateTriggerScore(context);

      if (Weight != null)
        result.Score = result.Score * Weight.Value;

      if (result.Score < 0.5)
        return null;

      ResponseContext rc = new ResponseContext(context.State, context.Input, result);
      return new Reaction(rc, this);
    }


    public static Random Randomizer = new Random();


    public string Invoke(ResponseContext context)
    {
      try
      {
        // Push new set of variables for matches $1...$N
        context.Variables.Push(new Dictionary<string, object>());

        foreach (var m in context.Match.Matches)
        {
          context.Variables[m.Key] = m.Value;
          context.Variables["$" + m.Key] = m.Value;
        }

        OutputExecutionContect ox_context = new OutputExecutionContect(context);

        foreach (OutputStatement output in OutputStatements)
        {
          output.Execute(ox_context);
        }

        string templateName = "default";
        if (ox_context.LastValue != null)
          templateName = ox_context.LastValue.TemplateName;

        string result = "";
        if (ox_context.OutputTemplates.ContainsKey(templateName))
        {
          IList<string> templates = ox_context.OutputTemplates[templateName];

          string selectedTemplate = templates[Randomizer.Next(templates.Count)];
          result = TextMerge.MergeTemplate(selectedTemplate, context.Variables);
        }

        foreach (var ea in ExpectedAnswers)
        {
          Rule r = ea(Domain);
          r.TimeToLive = 1;
          if (r.Weight != null)
            r.Weight *= 2;
          else
            r.Weight = 2;
        }

        return result;
      }
      finally
      {
        // Remove variables containing matches $1...$N for this rule invocation
        context.Variables.Pop();
      }
    }
  }
}
