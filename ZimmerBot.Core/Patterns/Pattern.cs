using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class Pattern
  {
    public List<PatternExpr> Expressions { get; protected set; }

    public double NumberOfWords { get; protected set; }

    protected Dictionary<string, double> WordInPatternProbability { get; set; }


    public Pattern(List<PatternExpr> expressions)
    {
      Condition.Requires(expressions, nameof(expressions)).IsNotNull();
      Expressions = expressions;

      NumberOfWords = Expressions.Count;
    }


    public void UpdateStatistics(double totalNumberOfWords)
    {
      WordInPatternProbability = new Dictionary<string, double>();

      // Count occurences
      foreach (var expr in Expressions)
      {
        string key = expr.Identifier;
        if (!WordInPatternProbability.ContainsKey(key))
          WordInPatternProbability[key] = 0;
        WordInPatternProbability[key] += 1;
      }

      // Calculate total probability of this pattern
      // (relative to the total number of words in all patterns)
      double PatternProbability = NumberOfWords / totalNumberOfWords;

      // Normalize
      foreach (string key in WordInPatternProbability.Keys.ToArray())
      {
        WordInPatternProbability[key] = PatternProbability * WordInPatternProbability[key] / NumberOfWords;
      }
    }


    public double CalculateProbability(ZTokenSequence input)
    {
      double prob = 1.0;

      foreach (var token in input)
      {
        string key = (token.Type == ZToken.TokenType.Entity
          ? "Entity:" + token.OriginalText
          : token.OriginalText);

        if (WordInPatternProbability.ContainsKey(key))
          prob *= WordInPatternProbability[key];
        else
          prob *= 0.01; // FIXME: what here?
      }

      return prob;
    }
  }
}
