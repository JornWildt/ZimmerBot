using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class OutputInitializationContext
  {
    public Rule ParentRule { get; protected set; }

    public OutputInitializationContext(Rule parentRule)
    {
      ParentRule = parentRule;
    }
  }
}
