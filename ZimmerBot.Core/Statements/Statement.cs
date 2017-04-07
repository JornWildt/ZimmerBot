using System;

namespace ZimmerBot.Core.Statements
{
  public abstract class Statement
  {
    public enum RepatableMode { Undefined, AutomaticSingle, AutomaticRepeatable, ForcedRepeatable, ForcedSingle }

    public string Id { get; protected set; }


    public Statement()
    {
      Id = Guid.NewGuid().ToString();
    }


    public abstract RepatableMode Repeatable { get; }

    public abstract void Initialize(StatementInitializationContext context);


    public abstract void Execute(StatementExecutionContect context);
  }
}
