using System;
using System.Linq;
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
      TriggerEvaluationContext context = BuildEvaluationContextFromInput(r.KnowledgeBase, text);
      return r.CalculateReaction(context);
    }


    protected string GetResponseFrom(Rule r, string text)
    {
      Reaction reaction = CalculateReaction(r, text);
      if (reaction == null)
        return null;
      string result = reaction.GenerateResponse().Aggregate((a, b) => a + "\n" + b);
      return result;
    }


    protected TriggerEvaluationContext BuildEvaluationContextFromInput(KnowledgeBase kb, string text)
    {
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence sequence = tokenizer.Tokenize(text);
      ZTokenSequence input = sequence.Statements[0];

      RequestState state = new RequestState();
      Session session = new Session("default");
      TriggerEvaluationContext context = 
        new TriggerEvaluationContext(
          new InputRequestContext(
            new RequestContext(kb, state, session),
            new Request(),
            input),
          null,
          executeScheduledRules: false);
      return context;
    }


    protected WRegex.MatchResult CalculateMatchResult(Trigger t, string text)
    {
      TriggerEvaluationContext context = BuildEvaluationContextFromInput(new KnowledgeBase(), text);
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
      Session session = new Session("default");
      RequestState state = new RequestState();
      ZTokenizer tokenizer = new ZTokenizer();
      ZStatementSequence stm = tokenizer.Tokenize(s);
      ZTokenSequence input = stm.Statements[0];
      TriggerEvaluationContext context =
        new TriggerEvaluationContext(
          new InputRequestContext(
            new RequestContext(new KnowledgeBase(), state, session),
            new Request(),
            input),
          null,
          executeScheduledRules: false);

      WRegex.MatchResult result = x.CalculateMatchResult(context, new EndOfSequenceWRegex());
      return result;
    }
  }
}
