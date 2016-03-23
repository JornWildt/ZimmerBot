using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.Core.Knowledge
{
  public class ResponseContext
  {
    public BotState State { get; protected set; }

    public ZTokenSequence Input { get; protected set; }

    public WRegex.MatchResult Match { get; protected set; }

    public ChainedDictionary<string, object> Variables { get; protected set; }


    public ResponseContext(BotState state, ZTokenSequence input, WRegex.MatchResult match)
    {
      // Both input and match can be null for scheduled, non-input based, responses
      Condition.Requires(state, "state").IsNotNull();

      State = state;
      Input = input;
      Match = match;

      // Register core bot state in variables
      Variables = new ChainedDictionary<string, object>(State.State);

      // Push new set of variables for matches
      Variables.Push(new Dictionary<string, object>());

      foreach (var m in match.Matches)
      {
        State[m.Key] = m.Value;
        State["$" + m.Key] = m.Value;
      }
    }
  }
}
