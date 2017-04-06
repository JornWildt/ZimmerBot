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

      List<Reaction> reactions = SelectReactions(context, result);
      return reactions;
    }


    private List<Reaction> SelectReactions(TriggerEvaluationContext context, MatchResult result)
    {
      // Add one reaction for each output statement
      List<Reaction> reactions = Statements
        .OfType<OutputTemplateStatement>()
        .Select(s => new Reaction(BuildResponseContext(context, result, s.Template), this, s.Template.Id))
        .ToList();

      // If no output statements are found then ensure we have at least one reaction (that does not identify an output)
      if (reactions.Count == 0)
        reactions.Add(new Reaction(new ResponseGenerationContext(context.InputContext, result), this, null));

      return reactions;
    }


    private ResponseGenerationContext BuildResponseContext(TriggerEvaluationContext context, MatchResult result, OutputTemplate template)
    {
      int outputUsageCount = context.InputContext.Session.GetUsageCount(template.Id);

      // Reduce the amount of repetition in output by lowering the reaction score by the number of times it has been used
      double score = result.Score * Math.Pow(0.99, outputUsageCount);

      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(score, result.Matches));
      return rc;
    }


    public static Random Randomizer = new Random();


    public List<string> Invoke(ResponseGenerationContext context, string outputId)
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
          string templateName = "default";
          if (ox_context.LastValue != null)
            templateName = ox_context.LastValue.TemplateName;

          if (ox_context.OutputTemplates.ContainsKey(templateName))
          {
            IList<OutputTemplate> templates = ox_context.OutputTemplates[templateName];
            OutputTemplate selectedTemplate = SelectTemplate(templates, templateName, outputId, context.InputContext.Session);

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

    private static OutputTemplate SelectTemplate(
      IList<OutputTemplate> templates, 
      string templateName, 
      string outputId, 
      Session session)
    {
      OutputTemplate selectedTemplate = null;

      if (outputId != null)
        selectedTemplate = templates.Where(t => t.Id == outputId).FirstOrDefault();

      if (selectedTemplate == null)
        selectedTemplate = selectedTemplate = templates[Randomizer.Next(templates.Count)];

      // This template got selected, remember that
      session.IncrementUsage(selectedTemplate.Id);

      return selectedTemplate;
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
