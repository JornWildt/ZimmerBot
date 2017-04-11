using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class StartTopicStatement : Statement
  {
    public override RepatableMode Repeatable { get { return RepatableMode.AutomaticRepeatable; } }

    public string Topic { get; protected set; }

    public StartTopicStatement(string topic)
    {
      Condition.Requires(topic, nameof(topic)).IsNotNullOrEmpty();

      Topic = topic;
    }


    public override void Execute(StatementExecutionContect context)
    {
      context.ResponseContext.Session.SetCurrentTopic(Topic);
    }


    public override void Initialize(StatementInitializationContext context)
    {
    }
  }
}
