using System;
using System.Collections.Generic;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Patterns
{
  public abstract class PatternExpr
  {
    public abstract string Identifier { get; }

    public abstract double Weight { get; }

    public abstract double ProbabilityFactor { get; }

    public virtual void UpdateEntityNumber(ref int entityNumber) { }

    public abstract void ExtractWordsForSpellChecker();

    public abstract void ExtractMatchValues(Dictionary<string, ZToken> matchValues, Queue<ZToken> entityTokens);

    public virtual void RegisterReferencedParameter(HashSet<string> p) { }

    public virtual ZTokenSequence ReduceInput(ZTokenSequence input, int myPos, List<PatternExpr> expressions, ref double reductionWeight) => input;

    public abstract double CalculateMatch(ZTokenSequence input, int myPos, List<PatternExpr> expressions);
  }
}
