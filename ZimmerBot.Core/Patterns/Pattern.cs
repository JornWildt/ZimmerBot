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

    protected double TotalNumberOfWords { get; set; }

    protected double PatternProbability { get; set; }

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
        _toString = Expressions.Select(e => $"{e}").Aggregate((a,b) => a + ", " + b);
      return _toString;
    }


    public bool HasParameterNamed(string p)
    {
      return ReferencedParameters.Contains(p);
    }


    public void ExpandExpressions(KnowledgeBase kb, List<Pattern> output)
    {
      Stack<PatternExpr> prefix = new Stack<PatternExpr>();
      ExpandExpressions(kb, output, prefix, 0);
    }


    protected void ExpandExpressions(KnowledgeBase kb, List<Pattern> output, Stack<PatternExpr> prefix, int pos)
    {
      if (pos >= Expressions.Count)
      {
        Pattern newPattern = new Pattern(prefix.Reverse().ToList());
        newPattern.RegisterParent(ParentPatternSet);
        output.Add(newPattern);
        return;
      }

      if (Expressions[pos] is ConceptPatternExpr cexpr)
      {
        if (!kb.Concepts.TryGetValue(cexpr.Word, out Concept c))
          throw new InvalidOperationException($"Could not find concept '{cexpr.Word}' for pattern '{this.ToString()}'");

        foreach (string word in c.ExpandPatterns())
        {
          string[] whitespace_expanded = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          foreach (string subw in whitespace_expanded)
            prefix.Push(new WordPatternExpr(subw));
          ExpandExpressions(kb, output, prefix, pos + 1);
          foreach (string subw in whitespace_expanded)
            prefix.Pop();
        }
      }
      else
      {
        prefix.Push(Expressions[pos]);
        ExpandExpressions(kb, output, prefix, pos + 1);
        prefix.Pop();
      }
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (var expr in Expressions)
      {
        expr.ExtractWordsForSpellChecker();
      }
    }


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
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

        RelatedExpression[key] = expr;
      }

      // All patterns have equal probability since this system have no apriori knowledge of actual usage patterns
      PatternProbability = 1.0 / totalNumberOfPatterns;

      // Normalize
      foreach (string key in WordInPatternProbability.Keys.ToArray())
      {
        WordInPatternProbability[key] 
          = Math.Log(PatternProbability * (WordInPatternProbability[key] + 1) / (NumberOfWords + totalNumberOfWords));
      }

      // Probability for completely unknown words
      UnknownWordProbability = Math.Log(PatternProbability * 1 / (NumberOfWords + TotalNumberOfWords));
    }


    public double CalculateProbability(ZTokenSequence input, out List<string> explanation)
    {
      // Probability is logarithmic! This means more negative values indicates smaller values between 0 and 1.
      // Normally probabilitis are multipled (making the end result smaller), but in log-space we add the (negative) values.
      double prob = 0.0;
      explanation = new List<string>();

      foreach (var token in input)
      {
        string key = (token.Type == ZToken.TokenType.Entity
          ? EntityPatternExpr.GetIdentifier(token.EntityClass, token.EntityNumber)
          : token.OriginalText);

        string wildcardKey = (token.Type == ZToken.TokenType.Entity
          ? EntityPatternExpr.GetIdentifier("", token.EntityNumber)
          : token.OriginalText);

        if (WordInPatternProbability.ContainsKey(key))
        {
          prob += WordInPatternProbability[key];
          explanation.Add($"+'{key}'");

          if (RelatedExpression.ContainsKey(key))
            prob *= RelatedExpression[key].ProbabilityFactor;
        }
        else if (WordInPatternProbability.ContainsKey(wildcardKey))
        {
          prob += WordInPatternProbability[wildcardKey];
          explanation.Add($"+'{wildcardKey}'");
        }
        else
        {
          prob += UnknownWordProbability;
          explanation.Add($"('{key}')");
        }
      }

      // Unmatched words in pattern counts negative
      if (Expressions.Count > input.Count)
        prob += (UnknownWordProbability/10) * (Expressions.Count - input.Count);


      return prob;
    }
  }
}
