using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class InputPipelineItem
  {
    #region Input

    public InputRequestContext Context { get; protected set; }

    public TriggerEvaluationContext EvaluationContext { get; protected set; }

    #endregion


    #region Output

    //public HashSet<string> SentenceTags { get; protected set; }

    public ReactionSet Reactions { get; protected set; }

    public List<string> Output { get; protected set; }

    #endregion


    public InputPipelineItem(InputRequestContext context)
    {
      Condition.Requires(context, nameof(context)).IsNotNull();

      Context = context;
      EvaluationContext = new TriggerEvaluationContext(context, executeScheduledRules: false);

      //SentenceTags = new HashSet<string>();
      Reactions = new ReactionSet();
      Output = new List<string>();
    }
  }
}
