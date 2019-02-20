using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Scheduler;
using ZimmerBot.Core.Statements;
using ZimmerBot.Core.TemplateParser;

namespace ZimmerBot.Core.Knowledge
{
  public abstract class Executable
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public List<Statement> Statements { get; protected set; }


    public static Random Randomizer = new Random();


    public Executable(KnowledgeBase kb, IEnumerable<RuleModifier> modifiers, IEnumerable<Statement> statements)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(statements, nameof(statements)).IsNotNull();

      KnowledgeBase = kb;

      Statements = new List<Statement>(statements);
      RegisterModifiers(modifiers);
    }


    public void RegisterModifiers(IEnumerable<RuleModifier> modifiers)
    {
      if (modifiers != null)
        foreach (var m in modifiers)
          m.Invoke(this);
    }


    public abstract Executable WithCondition(Expression c);


    public abstract Executable WithWeight(double w);



    public override string ToString()
    {
      return "Executable. Statements: #" + Statements.Count;
    }


    public abstract void SetupComplete();


    public virtual List<string> Invoke(ResponseGenerationContext context, string outputId)
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

            var output1 = selectedTemplate.Outputs.Select(t => TemplateUtility.Merge(t, new TemplateExpander(context)));
            List<string> output = SplitNewlinePlusToSeparateOutputStrings(output1);

            result.Add(AddMoreNotificationText(output[0], output.Count > 1));

            // Schedule remaining outputs delayed
            if (output.Count > 1)
            {
              DateTime at = DateTime.Now;
              for (int i = 1; i < output.Count; ++i)
              {
                string o = AddMoreNotificationText(output[i], i < output.Count - 1);
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

    private List<string> SplitNewlinePlusToSeparateOutputStrings(IEnumerable<string> response)
    {
      List<string> result = new List<string>();
      foreach (string r in response)
      {
        string[] multiLines = r.Split(new string[] { "\n+" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in multiLines)
          result.Add(line);
      }
      return result;
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
