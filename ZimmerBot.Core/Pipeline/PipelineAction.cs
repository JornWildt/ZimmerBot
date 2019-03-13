using System.Collections.Generic;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Pipeline
{
  public class PipelineAction : Executable
  {
    public Expression Condition { get; protected set; }

    public PipelineAction(KnowledgeBase kb, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, statements)
    {
      RegisterModifiers(modifiers);
    }


    public override void SetupComplete()
    {
    }


    public override Executable WithCondition(Expression c)
    {
      Condition = c;
      return this;
    }


    public override Executable WithWeight(double w)
    {
      return this;
    }
  }
}
