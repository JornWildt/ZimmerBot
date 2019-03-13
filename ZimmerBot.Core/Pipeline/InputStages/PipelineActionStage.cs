using System;
using System.Collections.Generic;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class PipelineActionStage : IPipelineHandler<InputPipelineItem>
  {
    protected List<PipelineAction> Actions { get; set; }


    public PipelineActionStage()
    {
      Actions = new List<PipelineAction>();
    }


    public void RegisterPipelineAction(PipelineAction action)
    {
      Actions.Add(action);
    }


    public void Handle(InputPipelineItem item)
    {
      ResponseGenerationContext responseContext =
        new ResponseGenerationContext(item.EvaluationContext.InputContext, new WordRegex.MatchResult(0.0));

      List<string> output = item.Output;

      foreach (var action in Actions)
      {
        bool canExecute = true;
        if (action.Condition != null)
        {
          ExpressionEvaluationContext eec = new ExpressionEvaluationContext(item.Context.Session, item.Context.Variables);
          object value = action.Condition.Evaluate(eec);
          if (!Expression.TryConvertToBool(value, out canExecute))
            throw new InvalidCastException($"Could not convert value '{value}' to bool in condition.");
        }

        if (canExecute)
        {
          List<string> response = action.Invoke(responseContext, null);

          if (response != null)
            output.AddRange(response);
        }
      }
    }
  }
}
