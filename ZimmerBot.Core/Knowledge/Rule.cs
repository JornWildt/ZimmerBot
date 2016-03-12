using System;
using System.Collections.Generic;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }


    protected HashSet<string> ParameterMap = new HashSet<string>();


    protected Func<TokenString, Func<string>> OutputGenerator { get; set; } // FIXME: better naming, cleanup


    public Rule(params string[] topics)
    {
      Trigger = new Trigger(topics);
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


    public Rule SetResponse(Func<TokenString, Func<string>> g)
    {
      OutputGenerator = g;
      return this;
    }


    public Rule SetResponse(string s)
    {
      OutputGenerator = i => () => s;
      return this;
    }


    public Reaction CalculateReaction(TokenString input)
    {
      double score = Trigger.CalculateTriggerScore(input);

      if (score < 2)
        return null;

      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();

      foreach (Token t in input)
        t.ExtractParameter(ParameterMap, generatorParameters);

      // Some parameter values are missing => ignore
      // FIXME: need only counting!
      if (ParameterMap.Count > generatorParameters.Count)
        return null;

      return new Reaction(score, OutputGenerator(input));
    }
  }
}
