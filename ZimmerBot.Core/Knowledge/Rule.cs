using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Scheduler;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.TemplateParser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public abstract class Rule
  {
    public string Id { get; protected set; }

    public KnowledgeBase KnowledgeBase { get; protected set; }

    public string Label { get; protected set; }

    public Topic RelatedTopic { get; protected set; }

    public List<Statement> Statements { get; protected set; }


    public Rule(KnowledgeBase kb, string label, Topic topic, IList<Statement> statements)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(statements, nameof(statements)).IsNotNull();

      Id = Guid.NewGuid().ToString();
      KnowledgeBase = kb;
      Label = label;
      RelatedTopic = topic;

      if (label != null)
        KnowledgeBase.RegisterRuleLabel(label, this);

      Statements = new List<Statement>(statements);
      StatementInitializationContext context = new StatementInitializationContext(this);
      foreach (Statement o in Statements)
        o.Initialize(context);
    }


    public override string ToString()
    {
      return string.IsNullOrEmpty(Label) ? "" : "<" + Label + ">. Statements: #" + Statements.Count;
    }


    public virtual void RegisterParentRule(Rule parentRule)
    {
    }


    public abstract IList<Reaction> CalculateReactions(TriggerEvaluationContext context, double weight);


    public static Random Randomizer = new Random();


    protected List<Reaction> SelectReactions(TriggerEvaluationContext context, MatchResult result)
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

      return reactions
             .Where(s => s.Score >= 0.5) // FIXME: Why exactly 0.5?
             .ToList();
    }


    protected ResponseGenerationContext MakeWeightedReaction(TriggerEvaluationContext context, MatchResult result, OutputTemplate template)
    {
      int outputUsageCount = context.InputContext.Session.GetUsageCount(template.Id);

      // Reduce the amount of repetition in output by lowering the reaction score by the number of times it has been used
      double score = result.Score * Math.Pow(0.99, outputUsageCount);
      //double score = result.Score * (outputUsageCount > 0 ? 0.0 : 1.0);

      ResponseGenerationContext rc = new ResponseGenerationContext(context.InputContext, new MatchResult(score, result.Matches));
      return rc;
    }


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
        // - But do allow other forms of input handling (for instance reacting to "stop writing messages")
        if (!context.Session.IsBusyWriting())
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
