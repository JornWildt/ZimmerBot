using System;
using System.Text.RegularExpressions;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class WRegex
  {
    public WRegexBase Expr { get; protected set; }

    protected NFANode Root { get; set; }


    public Type TypeOfExpr { get { return Expr.GetType(); } }

    static Regex LabelReducer = new Regex("[^\\w ]");


    public static WRegexBase BuildFromSpaceSeparatedString(string s, bool doStrip)
    {
      s = LabelReducer.Replace(s, " ");
      string[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      SequenceWRegex p = new SequenceWRegex();
      foreach (string word in words)
      {
        string w = word;
        p.Add(new LiteralWRegex(w));
      }
      return p;
    }


    public WRegex(WRegexBase expr)
    {
      Condition.Requires(expr, nameof(expr)).IsNotNull();
      Expr = expr;
    }


    public override string ToString()
    {
      return Expr.ToString();
    }


    public void ExtractWordsForSpellChecker()
    {
      Expr.ExtractWordsForSpellChecker();
    }


    public double CalculateSize()
    {
      return Expr.CalculateSize();
    }


    public MatchResult CalculateMatch(WRegexBase.EvaluationContext context)
    {
      if (Root == null)
      {
        Root = Expr.CalculateNFA(context);
      }

      MatchResult result = Expr.CalculateNFAMatch(Root, context);
      return result;
    }
  }
}
