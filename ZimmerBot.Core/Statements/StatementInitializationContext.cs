using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class StatementInitializationContext
  {
    public RuleBase ParentRule { get; protected set; }

    public StatementInitializationContext(RuleBase parentRule)
    {
      ParentRule = parentRule;
    }
  }
}
