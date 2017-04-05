using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Quartz;
using Quartz.Impl;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Id { get; protected set; }

    public KnowledgeBase KnowledgeBase { get; protected set; }

    public string Label { get; protected set; }

    public Trigger Trigger { get; protected set; }

    public double? Weight { get; protected set; }

    public List<Statement> Statements { get; protected set; }


    public Rule(KnowledgeBase kb, params object[] pattern)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(pattern, nameof(pattern)).IsNotNull();

      Id = Guid.NewGuid().ToString();
      KnowledgeBase = kb;
      Trigger = new Trigger(pattern);
    }


    public Rule WithLabel(string label)
    {
      Label = label;
      KnowledgeBase.RegisterRuleLabel(label, this);
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


    public Rule WithOutputStatements(IEnumerable<Statement> output)
    {
      Statements = new List<Statement>(output);
      StatementInitializationContext context = new StatementInitializationContext(this);
      foreach (Statement o in Statements)
        o.Initialize(context);
      return this;
    }


    public void RegisterScheduledJobs(IScheduler scheduler, string botId)
    {
      Trigger.RegisterScheduledJobs(scheduler, botId, Id);
    }


    public void RegisterParentRule(Rule parentRule)
    {
      Trigger.RegisterParentRule(parentRule);
    }


    public Reaction CalculateReaction(TriggerEvaluationContext context)
    {
      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return null;
      if (context.RestrictToRuleLabel != null && context.RestrictToRuleLabel != Label)
        return null;

      MatchResult result = Trigger.CalculateTriggerScore(context);

      if (Weight != null)
        result.Score = result.Score * Weight.Value;

      if (result.Score < 0.5)
        return null;

      // context.State, context.OriginalRequest, context.Input
      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, result);
      return new Reaction(rc, this);
    }


    public static Random Randomizer = new Random();


    public List<string> Invoke(ResponseGenerationContext context)
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

        StatementExecutionContect ox_context = new StatementExecutionContect(context);

        foreach (Statement output in Statements)
        {
          output.Execute(ox_context);
        }

        // TODO - the code below could be wrapped up in a statement and make it more script like - with the
        // possibility to generate output in multiple ways

        string templateName = "default";
        if (ox_context.LastValue != null)
          templateName = ox_context.LastValue.TemplateName;

        List<string> result = new List<string>();
        if (ox_context.OutputTemplates.ContainsKey(templateName))
        {
          IList<OutputTemplate> templates = ox_context.OutputTemplates[templateName];

          OutputTemplate selectedTemplate = templates[Randomizer.Next(templates.Count)];
          string[] output = selectedTemplate.Outputs.Select(t => TemplateUtility.Merge(t, new TemplateExpander(context))).ToArray();
          result.Add(output[0]);

          // Schedule remaining outputs delayed
          if (output.Length > 1)
          {
            DateTime at = DateTime.Now;
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            BotUtility.MarkAsBusy(context.Session);

            for (int i = 1; i < output.Length; ++i)
            {
              string o = output[i];
              at = at.AddSeconds(o.Length * AppSettings.MessageSequenceDelay.Value.TotalSeconds);
              Scheduler.AddDelayedMessage(scheduler, at, o, context,  i == output.Length-1);
            }
          }
        }

        result.AddRange(ox_context.AdditionalOutput);

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
