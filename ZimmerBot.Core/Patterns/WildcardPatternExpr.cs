using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public class WildcardPatternExpr : PatternExpr
  {
    public string ParameterName { get; protected set; }

    public override string Identifier => ParameterName;

    public override double Weight => 1.0;

    public override double ProbabilityFactor => 1.0;

    public WildcardPatternExpr(string name)
    {
      Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
      ParameterName = name;
    }


    public override string ToString()
    {
      return ParameterName;
    }


    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    protected Dictionary<string, bool> Words_Left = null;

    protected Dictionary<string, bool> Words_Right = null;

    protected ZToken MatchedValue = null;

    protected void Initialize(int myPos, List<PatternExpr> expressions)
    {
      // Initialize left/right locator words first time
      if (Words_Left == null)
      {
        Words_Left = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        Words_Right = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < expressions.Count; ++i)
        {
          if (i < myPos && expressions[i] is WordPatternExpr wl)
            Words_Left[wl.Word] = false;
          else if (i > myPos && expressions[i] is WordPatternExpr wr)
            Words_Right[wr.Word] = false;
        }
      }

      // Clear "word found" value for all words
      foreach (var key in Words_Left.Keys.ToArray())
        Words_Left[key] = false;
      foreach (var key in Words_Right.Keys.ToArray())
        Words_Right[key] = false;

      MatchedValue = null;
    }


    public override void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens)
    {
      if (MatchedValue != null)
        matchValues[ParameterName] = MatchedValue;
    }


    public override double CalculateMatch(ZTokenSequence input, int myPos, List<PatternExpr> expressions)
    {
      Initialize(myPos, expressions);

      int left = 0;
      int left_count = 0;
      int right = input.Count;
      int right_count = 0;
      for (int i = 0; i < input.Count; ++i)
      {
        int l = i;
        if (l < myPos && input[l].Type == ZToken.TokenType.Word 
            && Words_Left.ContainsKey(input[l].OriginalText) && !Words_Left[input[l].OriginalText])
        {
          Words_Left[input[l].OriginalText] = true;
          left = l;
          left_count++;
        }

        int r = input.Count - i - 1;
        if (r > myPos && input[r].Type == ZToken.TokenType.Word
            && Words_Right.ContainsKey(input[r].OriginalText) && !Words_Right[input[r].OriginalText])
        {
          Words_Right[input[r].OriginalText] = true;
          right = r;
          right_count++;
        }
      }

      if (left < right)
      {
        string result = "";
        for (int i = left+1; i < right; ++i)
          result += (i > left+1 ? " " : "") + input[i].OriginalText;
        MatchedValue = new ZToken(result);
      }

      return
        (double)((Words_Left.Count > 0 ? (double)left_count / Words_Left.Count : 1.0)
         + (Words_Right.Count > 0 ? (double)right_count / Words_Right.Count : 1.0)) / 2.0;
    }
  }
}
