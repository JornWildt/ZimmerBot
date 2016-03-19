using CuttingEdge.Conditions;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    protected ResponseContext Context { get; set; }

    protected Rule Rule { get; set; } // FIXME - better name


    public Reaction(ResponseContext context, Rule rule)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();
      Condition.Requires(rule, nameof(rule)).IsNotNull();

      Score = context.Match.Score;
      Context = context;
      Rule = rule;
    }


    public string GenerateResponse()
    {
      return Rule.Invoke(Context);
    }
  }
}
