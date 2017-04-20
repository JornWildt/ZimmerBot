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
    public PatternSet ParentPatternSet { get; protected set; }

    public List<PatternExpr> Expressions { get; protected set; }

    public double NumberOfWords { get; protected set; }

    protected Dictionary<string, double> WordInPatternProbability { get; set; }

    protected double TotalNumberOfWords { get; set; }

    protected double PatternProbability { get; set; }


    public Pattern(List<PatternExpr> expressions)
    {
      Condition.Requires(expressions, nameof(expressions)).IsNotNull();
      Expressions = expressions;

      NumberOfWords = Expressions.Count;
    }


    public void RegisterParent(PatternSet set)
    {
      ParentPatternSet = set;
    }


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      TotalNumberOfWords = totalNumberOfWords;
      WordInPatternProbability = new Dictionary<string, double>();

      // Count occurences
      foreach (var expr in Expressions)
      {
        string key = expr.Identifier;
        if (!WordInPatternProbability.ContainsKey(key))
          WordInPatternProbability[key] = 0;
        WordInPatternProbability[key] += 1;
      }

      // All patterns have equal probability since this system have no apriori knowledge of actual usage patterns
      PatternProbability = 1.0 / totalNumberOfPatterns;

      // Normalize
      foreach (string key in WordInPatternProbability.Keys.ToArray())
      {
        WordInPatternProbability[key] 
          = PatternProbability * (WordInPatternProbability[key] + 1) / (NumberOfWords + totalNumberOfWords);
      }
    }


    public double CalculateProbability(ZTokenSequence input)
    {
      double prob = 1.0;

      foreach (var token in input)
      {
        string key = (token.Type == ZToken.TokenType.Entity
          ? EntityPatternExpr.GetIdentifier(token.EntityClass)
          : token.OriginalText);

        string wildcardKey = (token.Type == ZToken.TokenType.Entity
          ? EntityPatternExpr.GetIdentifier("")
          : token.OriginalText);

        if (WordInPatternProbability.ContainsKey(key))
          prob *= WordInPatternProbability[key];
        else if (WordInPatternProbability.ContainsKey(wildcardKey))
          prob *= WordInPatternProbability[wildcardKey];
        else
          prob *= PatternProbability * 1 / (NumberOfWords + TotalNumberOfWords);
      }

      return prob;
    }
  }
}
