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


    public Reaction CalculateReaction(TokenString tokens)
    {
      Dictionary<string, string> generatorParameters = new Dictionary<string, string>();
      double score = 0;

      for (int i = 0; i < tokens.Count; ++i)
      {
        for (int j = 0; j < Matches.Length; ++j)
        {
          if (tokens[i].Matches(Matches[j]))
          {
            int distance = Math.Abs(j - i);
            if (distance < 3)
              score += 3 - distance;

            if (ParameterMap.ContainsKey(Matches[j]))
              generatorParameters[ParameterMap[Matches[j]]] = tokens[i].OriginalText;
          }
        }
      }

      score = score / tokens.Count;

      if (score < 2)
        return null;

      return new Reaction(score, Generator.Bind(generatorParameters));
    }
  }
}
