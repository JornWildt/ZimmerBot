using log4net;
using System;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class RegexTrigger : Trigger
  {
    private static ILog Logger = LogManager.GetLogger(typeof(RegexTrigger));

    public WRegex Regex { get; protected set; }

    public double RegexSize { get; protected set; }


    public RegexTrigger(WRegexBase pattern)
    {      
      Regex = new WRegex(pattern);
      RegexSize = pattern.CalculateSize();
    }


    public RegexTrigger(List<WRegexBase> patterns)
    {
      if (patterns == null || patterns.Count == 0 || patterns[0] == null)
      {
        throw new ArgumentNullException("patterns", "'patterns' is null or empty");
      }
      else if (patterns.Count == 1)
      {
        Regex = new WRegex(patterns[0]);
        RegexSize = Regex.CalculateSize();
      }
      else if (patterns.Count > 1)
      {
        Regex = new WRegex(new ChoiceWRegex(patterns));
        RegexSize = Regex.CalculateSize();
      }
    }


    public RegexTrigger(params object[] pattern)
    {
      if (pattern == null || pattern.Length == 0 || pattern[0] == null)
      {
        throw new ArgumentNullException("patterns", "'patterns' is null or empty");
      }
      else if (pattern.Length == 1)
      {
        Regex = new WRegex(GetRegex(pattern[0]));
        RegexSize = Regex.CalculateSize();
      }
      else if (pattern.Length > 1)
      {
        SequenceWRegex p = new SequenceWRegex();

        foreach (object t in pattern)
        {
          WRegexBase r = GetRegex(t);
          p.Add(r);
        }

        Regex = new WRegex(p);
        RegexSize = p.CalculateSize();
      }
    }


    private WRegexBase GetRegex(object t)
    {
      if (t is string)
        return new LiteralWRegex((string)t);
      else if (t is WRegexBase)
        return (WRegexBase)t;
      else if (t == null)
        throw new ArgumentNullException("t", "Null item in topics");
      else
        throw new InvalidOperationException(string.Format("Cannot add {0} ({1} as trigger predicate.", t, t.GetType()));
    }


    private string _toString;

    public override string ToString()
    {
      if (_toString == null)
        _toString = Regex.ToString() + $" [Size: {RegexSize}]";
      return _toString;
    }


    public override void ExtractWordsForSpellChecker()
    {
      Regex.ExtractWordsForSpellChecker();
    }


    public override MatchResult CalculateTriggerScore(TriggerEvaluationContext context)
    {
      double conditionModifier = CalculateConditionModifier(context);

      context.CurrentRepetitionIndex = 1;
      MatchResult result = null;

      if (Regex != null)
      {
        if (context.InputContext.Input != null)
        {
          foreach (ZTokenSequence input in context.InputContext.Input)
          {
            var subResult = Regex.CalculateMatch(new WRegexBase.EvaluationContext(input, 0, 99999));
            if (result == null || subResult.Score > result.Score)
              result = subResult;
          }
        }
        else
          result = new MatchResult(0);
      }
      else
      {
        throw new NotImplementedException("Why did we end up here?");
      }

      double totalScore = conditionModifier * result.Score * RegexSize;

      if (result.Score > 0)
        BotUtility.EvaluationLogger.Debug($"Matched {Regex}. Score = {totalScore}");

      return new MatchResult(result, totalScore);
    }
  }
}
