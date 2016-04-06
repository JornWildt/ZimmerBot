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
    protected ConfigurationParser CfgParser { get; set; } = new ConfigurationParser();

    protected Reaction CalculateReaction(Rule r, string text)
    {
      EvaluationContext context = BuildEvaluationContextFromInput(text);
      return r.CalculateReaction(context);
    }


    protected string GetResponseFrom(Rule r, string text)
    {
      Reaction reaction = CalculateReaction(r, text);
      if (reaction == null)
        return null;
      string result = reaction.GenerateResponse();
      return result;
    }


    protected EvaluationContext BuildEvaluationContextFromInput(string text)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence sequence = tokenizer.Tokenize(text);
      ZTokenSequence input = sequence.Statements[0];

      SessionState state = new SessionState();
      EvaluationContext context = new EvaluationContext(state, new Request(), input, null, executeScheduledRules: false);
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


    protected KnowledgeBase ParseKnowledgeBase(string s)
    {
      KnowledgeBase kb = new KnowledgeBase();
      CfgParser.ParseConfigurationString(kb, s);
      return kb;
    }


    protected Rule ParseRule(string s)
    {
      KnowledgeBase kb = ParseKnowledgeBase(s);
      Assert.AreEqual(1, kb.Rules.Count);
      Rule r = kb.Rules[0];
      return r;
    }


    protected T ParseRuleAndGetRootWRegex<T>(string s)
      where T : WRegex
    {
      Rule r = ParseRule(s);
      Assert.IsInstanceOf<T>(r.KnowledgeBase.Rules[0].Trigger.Regex);
      T seq = (T)r.KnowledgeBase.Rules[0].Trigger.Regex;

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
      SessionState state = new SessionState();
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence stm = tokenizer.Tokenize(s);
      ZTokenSequence input = stm.Statements[0];
      EvaluationContext context = new EvaluationContext(state, new Request(), input, null, false);
      WRegex.MatchResult result = x.CalculateMatchResult(context, new EndOfSequenceWRegex());
      return result;
    }
  }
}
