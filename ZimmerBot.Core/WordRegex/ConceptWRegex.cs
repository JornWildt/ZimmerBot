using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  /// <summary>
  /// A WRegex that referes to a concept (which in turn contains a wregex which is used).
  /// </summary>
  public class ConceptWRegex : WRegex
  {
    public Concept Concept { get; protected set; }


    public ConceptWRegex(KnowledgeBase kb, string cword)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(cword, nameof(cword)).IsNotNull();

      if (cword.StartsWith("%"))
        cword = cword.Substring(1);

      if (!kb.Concepts.ContainsKey(cword))
        throw new InvalidOperationException($"The concept reference '{cword}' could not be found.");

      Concept = kb.Concepts[cword];
    }


    public override double CalculateSize()
    {
      return Concept.Choices.CalculateSize();
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      context.MatchNames.Push(Concept.Name);
      NFAFragment f = Concept.Choices.CalculateNFAFragment(context);
      context.MatchNames.Pop();

      return f;
    }
  }
}
