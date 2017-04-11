using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class StatementInitializationContext
  {
    public Rule ParentRule { get; protected set; }

    public StatementInitializationContext(Rule parentRule)
    {
      ParentRule = parentRule;
    }
  }
}
