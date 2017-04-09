using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NUnit.Framework;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Tests
{
  public class TestHelper : TestHelperBase
  {
    internal static ILog Logger = LogManager.GetLogger(typeof(TestHelper));

    protected ConfigurationParser CfgParser { get; set; } = new ConfigurationParser();


    protected override void SetUp()
    {
      base.SetUp();
      SessionManager.ClearSessions();
    }


    protected IList<Reaction> CalculateReactions(Rule r, string text)
    {
      TriggerEvaluationContext context = BuildEvaluationContextFromInput(r.KnowledgeBase, text);
      return r.CalculateReactions(context);
    }


    protected string GetResponseFrom(Rule r, string text)
    {
      IList<Reaction> reactions = CalculateReactions(r, text);
      return GetResponseFrom(reactions);
    }


    protected string GetResponseFrom(IList<Reaction> reactions)
    {
      if (reactions == null || reactions.Count() == 0)
        return null;
      string result = reactions.Select(rsp => rsp.GenerateResponse().Aggregate((a, b) => a + "\n" + b)).Aggregate((a, b) => a + "|" + b);
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
          executeScheduledRules: false);
      return context;
    }


    protected MatchResult CalculateMatchResult(Trigger t, string text)
    {
      TriggerEvaluationContext context = BuildEvaluationContextFromInput(new KnowledgeBase(), text);
      MatchResult result = t.CalculateTriggerScore(context);
      return result;
    }


    protected double CalculateScore(Trigger t, string text)
    {
      MatchResult result = CalculateMatchResult(t, text);
      Console.WriteLine("Score for '{0}' = {1}.", text, result.Score);
      return Math.Round(result.Score, 4);
    }


    protected KnowledgeBase ParseKnowledgeBase(string s)
    {
      KnowledgeBase kb = new KnowledgeBase();
      kb.Initialize(KnowledgeBase.InitializationMode.Clear);
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


    protected Topic ParseTopic(string s)
    {
      KnowledgeBase kb = ParseKnowledgeBase(s);
      Assert.AreEqual(1, kb.Topics.Count);
      Topic t = kb.Topics.First().Value;
      return t;
    }


    protected T ParseRuleAndGetRootWRegex<T>(string s)
      where T : WRegexBase
    {
      Rule r = ParseRule(s);
      Assert.IsInstanceOf<T>(r.KnowledgeBase.Rules[0].Trigger.Regex.Expr);
      T expr = (T)r.KnowledgeBase.Rules[0].Trigger.Regex.Expr;

      return expr;
    }


    protected void VerifyMatch(WRegexBase x, string s, Dictionary<string,string> expectedMatches = null)
    {
      MatchResult result = CalculateMatch(x, s);
      Assert.IsTrue(result.Score > 0.9, $"The input '{s}' does not match with the wregex.");

      if (expectedMatches != null)
      {
        foreach (var item in expectedMatches)
        {
          Assert.IsTrue(result.Matches.ContainsKey(item.Key), $"The key {item.Key} was not found in matches");
          Assert.AreEqual(expectedMatches[item.Key], result.Matches[item.Key], $"Wrong value for key {item.Key}" );
        }
      }
    }


    protected void VerifyNoMatch(WRegexBase x, string s)
    {
      MatchResult result = CalculateMatch(x, s);
      Assert.IsTrue(result.Score < 0.9, $"The input '{s}' unexpectedly match with the wregex.");
    }


    protected MatchResult CalculateMatch(WRegexBase x, string s)
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
          executeScheduledRules: false);

      //WRegex.MatchResult result = x.CalculateMatchResult(context, new EndOfSequenceWRegex());
      MatchResult result = x.CalculateNFAMatch(context);
      return result;
    }
  }
}
