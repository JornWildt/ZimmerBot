using System;

namespace ZimmerBot.Core.ConfigParser
{
  public abstract class OutputStatement
  {
    public string Id { get; protected set; }


    public OutputStatement()
    {
      Id = Guid.NewGuid().ToString();
    }


    public abstract void Execute(OutputExecutionContect context);
  }
}
