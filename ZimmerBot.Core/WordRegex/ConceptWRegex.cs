using System;
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


    public override MatchResult CalculateMatchResult(EvaluationContext context, WRegex lookahead)
    {
      MatchResult result = Concept.Choices.CalculateMatchResult(context, lookahead);
      return result.RegisterMatch(Concept.Name, result.MatchedText);
    }


    public override double CalculateSize()
    {
      return Concept.Choices.CalculateSize();
    }


    public override WRegex GetLookahead()
    {
      return Concept.Choices.GetLookahead();
    }
  }
}
