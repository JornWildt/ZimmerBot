using System.Collections.Generic;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Pipeline.InputStages
{
  public class ConceptTaggingStage : IPipelineHandler<InputPipelineItem>
  {
    public void Handle(InputPipelineItem item)
    {
      if (item.Context.Input == null)
        return;

      // Take a copy of the input list since we are adding to it in the loop
      foreach (ZTokenSequence input in item.Context.Input.ToArray())
      {
        FindConcepts(item.Context.KnowledgeBase.Concepts.Values, input, 0, item.Context.Input);
      }
    }


    protected void FindConcepts(ICollection<Concept> concepts, ZTokenSequence input, int start, List<ZTokenSequence> output)
    {
      foreach (var concept in concepts)
      {
        for (int i = start; i < input.Count; ++i)
        {
          for (int j = input.Count; j > i; --j)
          {
            bool isConcept = concept.IsConceptMatch(input, i, j);
            if (isConcept)
            {
              ZTokenSequence result = input.CompactConcept(concept, i, j);
              output.Add(result);

              int next = i + 1;

              if (next < result.Count)
                FindConcepts(concepts, result, next, output);

              // This is a greedy algortihm, so do not try to match smaller combinations
              i = j;
            }
          }
        }
      }
    }
  }
}
