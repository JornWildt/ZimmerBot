using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.Knowledge
{
  public class Reaction
  {
    public double Score { get; protected set; }

    public ResponseGenerationContext Context { get; protected set; }

    public Rule Rule { get; protected set; }

    public string OutputIdentifier { get; protected set; }


    public Reaction(ResponseGenerationContext context, Rule rule, string outputId)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();
      Condition.Requires(rule, nameof(rule)).IsNotNull();

      Score = context.Match.Score;
      Context = context;
      Rule = rule;
      OutputIdentifier = outputId;
    }


    public List<string> GenerateResponse()
    {
      return Rule.Invoke(Context);
    }
  }
}
