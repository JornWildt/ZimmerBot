using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class Pattern
  {
    public PatternSet ParentPatternSet { get; protected set; }

    public List<PatternExpr> Expressions { get; protected set; }

    public HashSet<string> ReferencedParameters { get; protected set; }

    public double NumberOfWords { get; protected set; }

    protected Dictionary<string, double> WordInPatternProbability { get; set; }

    protected Dictionary<string, PatternExpr> RelatedExpression { get; set; }

    public bool HasWildcardExpression { get; protected set; }

    protected double TotalNumberOfWords { get; set; }

    protected double UnknownWordProbability { get; set; }


    public Pattern(List<PatternExpr> expressions)
    {
      Condition.Requires(expressions, nameof(expressions)).IsNotNull();
      Expressions = expressions;

      NumberOfWords = Expressions.Count;
      ReferencedParameters = new HashSet<string>();

      int entityNum = 1;
      foreach (var expr in Expressions)
      {
        expr.UpdateEntityNumber(ref entityNum);
        expr.RegisterReferencedParameter(ReferencedParameters);

        if (expr is WildcardPatternExpr)
          HasWildcardExpression = true;
      }

      RelatedExpression = new Dictionary<string, PatternExpr>(StringComparer.OrdinalIgnoreCase);
    }


    public void RegisterParent(PatternSet set)
    {
      ParentPatternSet = set;
    }


    private string _toString = null;

    public override string ToString()
    {
      if (_toString == null)
        _toString = Expressions.Select(e => $"{e}").Aggregate((a, b) => a + ", " + b);
      return _toString;
    }


    public bool HasParameterNamed(string p)
    {
      return ReferencedParameters.Contains(p);
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (var expr in Expressions)
      {
        expr.ExtractWordsForSpellChecker();
      }
    }


    public void InitializeStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      TotalNumberOfWords = totalNumberOfWords;
      WordInPatternProbability = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

      // Count occurences
      foreach (var expr in Expressions)
      {
        string key = expr.Identifier;
        if (!WordInPatternProbability.ContainsKey(key))
          WordInPatternProbability[key] = 0;
        WordInPatternProbability[key] += 1;

        if (!ParentPatternSet.WordInPatternSetProbability.ContainsKey(key))
          ParentPatternSet.WordInPatternSetProbability[key] = 0;
        ParentPatternSet.WordInPatternSetProbability[key] += 1;

        RelatedExpression[key] = expr;
      }
    }


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      // All patterns have equal probability since this system have no apriori knowledge of actual usage patterns
      double patternProbability = 1.0 / totalNumberOfPatterns;

      // Normalize
      foreach (string key in WordInPatternProbability.Keys.ToArray())
      {
        WordInPatternProbability[key]
          = Math.Log(patternProbability * (WordInPatternProbability[key] + 1) / (totalNumberOfWords));
      }

      // Probability for completely unknown words
      UnknownWordProbability = Math.Log(patternProbability * 1 / (totalNumberOfWords * 3));
    }


    // Original way of calculating probability - or score - for a single pattern.
    public double CalculateProbability(ZTokenSequence input, out List<string> explanation)
    {
      // Probability is logarithmic! This means more negative values indicates smaller values between 0 and 1.
      // Normally probabilitis are multipled (making the end result smaller), but in log-space we add the (negative) values.
      double prob = 0.0;
      explanation = new List<string>();

      // Let wildcard expressions reduce the input, going from multiple wildcard-matched tokens to a single large token.
      double reductionWeight = 0.0;
      for (int i = 0; i < Expressions.Count; ++i)
        input = Expressions[i].ReduceInput(input, i, Expressions, ref reductionWeight);

      //IDictionary<string, double> probs = ParentPatternSet.WordInPatternSetProbability;
      IDictionary<string, double> probs = WordInPatternProbability;
      
      IDictionary<string, double> parentProbs = ParentPatternSet.WordInPatternSetProbability;

      double unknownWordProp = ParentPatternSet.UnknownWordProbability;
      //double unknownWordProp = UnknownWordProbability;

      int matchCount = 0;

      foreach (var token in input)
      {
        string key = token.GetKey();
        string untypedKey = token.GetUntypedKey();

        if (probs.ContainsKey(key))
        {
          prob += probs[key];
          explanation.Add($"+'{key}'({probs[key]})");

          prob += (double)(token.Size - 1) * (probs[key] + unknownWordProp) / 2.0;

          if (RelatedExpression.ContainsKey(key))
            prob *= RelatedExpression[key].ProbabilityFactor;

          ++matchCount;
        }
        else if (probs.ContainsKey(untypedKey))
        {
          prob += probs[untypedKey];
          explanation.Add($"+'{untypedKey}'({probs[untypedKey]})");
          ++matchCount;
        }
        else if (parentProbs.ContainsKey(key))
        {
          // If another pattern in this pattern contains the word, then it is still an unknown word - but with a slightly better chance of matching
          prob += unknownWordProp * 0.95;
          explanation.Add($"('{key}'({unknownWordProp * 0.95}))");
        }
        else
        {
          prob += unknownWordProp;
          explanation.Add($"('{key}'({unknownWordProp}))");
        }
      }

      // Unmatched words in pattern counts negative
      if (Expressions.Count > matchCount)
        prob += (unknownWordProp / 10) * (Expressions.Count - matchCount);

      //prob += reductionWeight > 0 ? Math.Log(reductionWeight) : unknownWordProp * 10;
      //prob += reductionWeight * unknownWordProp;

      return prob;
    }


    // Later way of calculating score for a pattern
    public double CalculateScore(ZTokenSequence input, out List<string> explanation)
    {
      double score = 0.0;
      explanation = new List<string>();

      // Let wildcard expressions reduce the input, going from multiple wildcard-matched tokens to a single large token.
      double reductionWeight = 1.0;
      for (int i = 0; i < Expressions.Count; ++i)
        input = Expressions[i].ReduceInput(input, i, Expressions, ref reductionWeight);

      for (int i=0; i<Expressions.Count; ++i)
      {
        PatternExpr expr = Expressions[i];

        double exprScore = expr.CalculateMatch(input, i, Expressions);
        score += exprScore;

        explanation.Add("FIXME");
      }

      return score;
    }
  }
}
