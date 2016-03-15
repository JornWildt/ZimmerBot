using System;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }


    protected HashSet<string> ParameterMap = new HashSet<string>();


    protected Func<WRegex.MatchResult, Func<string>> OutputGenerator { get; set; } // FIXME: better naming, cleanup


    public Rule(params object[] topics)
    {
      Trigger = new Trigger(topics);
    }


    public Rule(StateWRegex p)
    {
      Trigger = new Trigger(p);
    }


    public Rule Describe(string description)
    {
      Description = description;
      return this;
    }


    public Rule Parameter(string name)
    {
      ParameterMap.Add(name);
      return this;
    }


    public Rule Response(Func<WRegex.MatchResult, Func<string>> g)
    {
      OutputGenerator = g;
      return this;
    }


    public Rule Response(string s)
    {
      OutputGenerator = i => () => TextMerge.MergeTemplate(s, i.Matches);
      return this;
    }


    public Reaction CalculateReaction(EvaluationContext context)
    {
      WRegex.MatchResult result = Trigger.CalculateTriggerScore(context);

      if (result.Score < 0.5)
        return null;

      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();

      foreach (ZToken t in context.Input)
        t.ExtractParameter(ParameterMap, generatorParameters);

      // Some parameter values are missing => ignore
      // FIXME: need only counting!
      if (ParameterMap.Count > generatorParameters.Count)
        return null;

      return new Reaction(result.Score, OutputGenerator(result));
    }
  }
}
