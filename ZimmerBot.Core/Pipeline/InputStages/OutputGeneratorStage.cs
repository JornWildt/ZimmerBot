using System;
using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class OutputGeneratorStage : IPipelineHandler<InputPipelineItem>
  {
    static Random Randomizer = new Random();


    public void Handle(InputPipelineItem item)
    {
      RequestState state = item.Context.State;
      List<string> output = item.Output;

      if (item.Reactions != null && item.Reactions.Count > 0)
      {
        // Select a random reaction
        Reaction r = item.Reactions[Randomizer.Next(item.Reactions.Count)];
        BotUtility.EvaluationLogger.Debug($"Selected reaction {r.Rule.ToString()}");
        List<string> response = r.GenerateResponse();

        output.AddRange(response);

        // Remember last used rule for handling of answers
        state[StateKeys.SessionStore][StateKeys.LastRuleId] = r.Rule.Id;

        foreach (string s in response)
          BotUtility.DiaLogger.Info($"[{item.Context.Session.SessionId}] {s}");
      }
      else
      {
        // Only add "???" for unrecognized input when there actually is some input
        if (item.Context.Input != null)
          output.Add("???");

        BotUtility.DiaLogger.Info($"[{item.Context.Session.SessionId}] No suitable response found");
        state[StateKeys.SessionStore][StateKeys.LastRuleId] = null;
      }
    }
  }
}
