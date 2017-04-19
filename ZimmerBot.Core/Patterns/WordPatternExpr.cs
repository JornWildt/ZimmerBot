using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Patterns
{
  public class WordPatternExpr : PatternExpr
  {
    public string Word { get; protected set; }


    public WordPatternExpr(string word)
    {
      Condition.Requires(word, nameof(word)).IsNotNullOrWhiteSpace();

      Word = word;
    }
  }
}
