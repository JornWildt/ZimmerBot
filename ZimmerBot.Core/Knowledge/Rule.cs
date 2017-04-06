using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Quartz;
using Quartz.Impl;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Scheduler;
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


    public Rule WithStatements(IEnumerable<Statement> statements)
    {
      Statements = new List<Statement>(statements);
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


    public IList<Reaction> CalculateReaction(TriggerEvaluationContext context)
    {
      if (context.RestrictToRuleId != null && context.RestrictToRuleId != Id)
        return new List<Reaction>();
      if (context.RestrictToRuleLabel != null && context.RestrictToRuleLabel != Label)
        return new List<Reaction>();

      MatchResult result = Trigger.CalculateTriggerScore(context);

      if (Weight != null)
        result.Score = result.Score * Weight.Value;

      if (result.Score < 0.5)
        return new List<Reaction>();

      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, result);

      return Statements
        .Select(s => s is OutputTemplateStatement
          ? new Reaction(rc, this, ((OutputTemplateStatement)s).Template.Identifier)
          : new Reaction(rc, this, null)).ToList();
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

        foreach (Statement stmt in Statements)
        {
          stmt.Execute(ox_context);
        }

        List<string> result = new List<string>();

        // Do not generate output if bot is busy (meaning "busy simulating writing")
        // - But do allow other forms of input handling (for instance reacting to "stop messages")
        if (!BotUtility.IsBusy(context.Session))
        {
          // TODO - the code below could be wrapped up in a statement and make it more script like - with the
          // possibility to generate output in multiple ways

          string templateName = "default";
          if (ox_context.LastValue != null)
            templateName = ox_context.LastValue.TemplateName;

          if (ox_context.OutputTemplates.ContainsKey(templateName))
          {
            IList<OutputTemplate> templates = ox_context.OutputTemplates[templateName];

            OutputTemplate selectedTemplate = templates[Randomizer.Next(templates.Count)];

            string[] output = selectedTemplate.Outputs.Select(t => TemplateUtility.Merge(t, new TemplateExpander(context))).ToArray();
            result.Add(AddMoreNotificationText(output[0], output.Length > 1));

            // Schedule remaining outputs delayed
            if (output.Length > 1)
            {
              DateTime at = DateTime.Now;
              for (int i = 1; i < output.Length; ++i)
              {
                string o = AddMoreNotificationText(output[i], i < output.Length - 1);
                at += TimeSpan.FromSeconds(o.Length * AppSettings.MessageSequenceDelay.Value.TotalSeconds);
                ScheduleHelper.AddDelayedMessage(at, o, context);
              }
            }
          }
        }

        return result;
      }
      finally
      {
        // Remove variables containing matches $1...$N for this rule invocation
        context.Variables.Pop();
      }
    }


    protected string AddMoreNotificationText(string text, bool hasMore)
    {
      if (hasMore)
        return text + AppSettings.MessageSequenceNotoficationText;
      else
        return text;
    }
  }
}
