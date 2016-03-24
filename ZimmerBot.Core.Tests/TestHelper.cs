using System;
using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  public class TestHelper : TestHelperBase
  {
    protected ConfigurationParser CfgParser = new ConfigurationParser();


    protected Reaction CalculateReaction(Rule r, string text)
    {
      EvaluationContext context = BuildEvaluationContextFromInput(text);
      return r.CalculateReaction(context);
    }


    protected string GetResponseFrom(Rule r, string text)
    {
      Reaction reaction = CalculateReaction(r, text);
      string result = reaction.GenerateResponse();
      return result;
    }


    protected EvaluationContext BuildEvaluationContextFromInput(string text)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence sequence = tokenizer.Tokenize(text);
      ZTokenSequence input = sequence.Statements[0];

      BotState state = new BotState();
      EvaluationContext context = new EvaluationContext(state, input, null, executeScheduledRules: false);
      return context;
    }


    protected WRegex.MatchResult CalculateMatchResult(Trigger t, string text)
    {
      EvaluationContext context = BuildEvaluationContextFromInput(text);
      WRegex.MatchResult result = t.CalculateTriggerScore(context);
      return result;
    }


    protected double CalculateScore(Trigger t, string text)
    {
      WRegex.MatchResult result = CalculateMatchResult(t, text);
      Console.WriteLine("Score for '{0}' = {1}.", text, result.Score);
      return Math.Round(result.Score, 4);
    }


    protected Domain ParseDomain(string s)
    {
      KnowledgeBase kb = new KnowledgeBase();
      Domain d = kb.NewDomain("Test");
      CfgParser.ParseConfigurationString(d, s);
      return d;
    }


    protected Rule ParseRule(string s)
    {
      Domain d = ParseDomain(s);
      Assert.AreEqual(1, d.Rules.Count);
      Rule r = d.Rules[0];
      return r;
    }


    protected T ParseRuleAndGetRootWRegex<T>(string s)
      where T : WRegex
    {
      Rule r = ParseRule(s);
      Assert.IsInstanceOf<T>(r.Domain.Rules[0].Trigger.Regex);
      T seq = (T)r.Domain.Rules[0].Trigger.Regex;

      return seq;
    }


    protected void VerifyMatch(WRegex x, string s)
    {
      WRegex.MatchResult result = CalculateMatch(x, s);
      Assert.IsTrue(result.Score > 0.9, $"The input '{s}' does not match with the wregex.");
    }


    protected void VerifyNoMatch(WRegex x, string s)
    {
      WRegex.MatchResult result = CalculateMatch(x, s);
      Assert.IsTrue(result.Score < 0.9, $"The input '{s}' unexpectedly match with the wregex.");
    }


    protected WRegex.MatchResult CalculateMatch(WRegex x, string s)
    {
      BotState state = new BotState();
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence stm = tokenizer.Tokenize(s);
      ZTokenSequence input = stm.Statements[0];
      EvaluationContext context = new EvaluationContext(state, input, null, false);
      WRegex.MatchResult result = x.CalculateMatchResult(context, new EndOfSequenceWRegex());
      return result;
    }
  }
}
