using System.Collections.Generic;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Responses;


namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Description { get; protected set; }

    protected Trigger Trigger { get; set; }


    protected Dictionary<string, string> ParameterMap = new Dictionary<string, string>();


    public ResponseGenerator Generator { get; protected set; }


    public Rule(params string[] topics)
    {
      Trigger = new Trigger(topics);
    }


    public Rule Describe(string description)
    {
      Description = description;
      return this;
    }


    public Rule Parameter(string name, string match)
    {
      ParameterMap[match] = name;
      return this;
    }


    public Rule SetResponse(ResponseGenerator g)
    {
      Generator = g;
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
      if (ParameterMap.Count > generatorParameters.Count)
        return null;

      return new Reaction(score, Generator.Bind(generatorParameters));
    }
  }
}
