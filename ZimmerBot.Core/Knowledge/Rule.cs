using System;
using System.Collections.Generic;
using ZimmerBot.Core.Language;

namespace ZimmerBot.Core.Knowledge
{
  public class Rule
  {
    public string Description { get; protected set; }

    protected string[] Matches { get; set; } // => refactor to Trigger class

    protected Dictionary<string, string> ParameterMap = new Dictionary<string, string>();


    public ResponseGenerator Generator { get; protected set; }


    public Rule(params string[] matches)
    {
      Matches = matches;
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
      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();
      int matches = 0;
      double score = 0;

      for (int i = 0; i < input.Count; ++i)
      {
        bool gotMatch = false;

        for (int j = 0; j < Matches.Length; ++j)
        {
          if (input[i].Matches(Matches[j]))
          {
            gotMatch = true;
            int distance = Math.Abs(j - i);
            if (distance < 3)
              score += 3 - distance;

            if (ParameterMap.ContainsKey(Matches[j]))
              generatorParameters[ParameterMap[Matches[j]]] = input[i].OriginalText;
          }
        }

        if (gotMatch)
          ++matches;
      }

      // Normalize score relative to word count
      score = score / input.Count;

      // Prefer longer matches over short ones
      score += matches;

      if (score < 2)
        return null;

      // Some parameter values are missing => ignore
      if (ParameterMap.Count > generatorParameters.Count)
        return null;

      return new Reaction(score, Generator.Bind(generatorParameters));
    }
  }
}
