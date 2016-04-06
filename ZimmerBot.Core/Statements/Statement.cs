using System;

namespace ZimmerBot.Core.Statements
{
  public abstract class Statement
  {
    public string Id { get; protected set; }


    public Statement()
    {
      Id = Guid.NewGuid().ToString();
    }


    public abstract void Initialize(StatementInitializationContext context);


    public abstract void Execute(StatementExecutionContect context);
  }
}
