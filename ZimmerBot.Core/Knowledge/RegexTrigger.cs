using System;
using System.Collections.Generic;
using Quartz;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.Knowledge
{
  public class RegexTrigger : Trigger
  {
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


    public override string ToString()
    {
      return Regex.ToString() + $" [Size: {RegexSize}]";
    }


    public override void ExtractWordsForSpellChecker()
    {
      Regex.ExtractWordsForSpellChecker();
    }


    public override void RegisterScheduledJobs(IScheduler scheduler, string botId, string ruleId)
    {
      // Ignore
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
        //if (context.InputContext.Input != null)
        //  result = new MatchResult(0);
        //else
        //  result = new MatchResult(1);
      }

      double totalScore = conditionModifier * result.Score * RegexSize;

      return new MatchResult(result, totalScore);
    }
  }
}
