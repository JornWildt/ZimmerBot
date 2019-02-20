using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class StartTopicStatement : Statement
  {
    public override RepatableMode Repeatable { get { return RepatableMode.AutomaticRepeatable; } }

    public string Topic { get; protected set; }

    public bool Restart { get; protected set; }


    public StartTopicStatement(string topic, bool restart)
    {
      Condition.Requires(topic, nameof(topic)).IsNotNullOrEmpty();

      Topic = topic;
      Restart = restart;
    }


    public override void Execute(StatementExecutionContect context)
    {
      context.ResponseContext.Session.SetCurrentTopic(Topic);
      if (Restart)
        context.ResponseContext.Session.SetTopicRuleIndex(Topic, 0);
    }


    public override void Initialize(StatementInitializationContext context)
    {
    }
  }
}
