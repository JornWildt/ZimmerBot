using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class ContinueStatement : Statement
  {
    public Continuation ContinuationChoice { get; protected set; }


    public ContinueStatement()
    {
      // Empty continuation means "continue and trigger all the "default" rules (topic rules or * rules).
      // Empty does not mean "continue and match rules with empty triggers".
      ContinuationChoice = new Continuation(Continuation.ContinuationEnum.Input, "somebogustextnoonewillenterhiwiqjedoih");
    }


    public override RepatableMode Repeatable
    {
      get { return RepatableMode.Undefined; }
    }


    public ContinueStatement(Continuation c)
    {
      Condition.Requires(c, nameof(c)).IsNotNull();
      ContinuationChoice = c;
    }


    public ContinueStatement(List<string> target)
    {
      Condition.Requires(target, nameof(target)).IsNotNull();

      string input = target.Aggregate((a,b) => a + " " + b);
      ContinuationChoice = new Continuation(Continuation.ContinuationEnum.Input, input);
    }


    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }


    public override void Execute(StatementExecutionContect context)
    {
      context.Continue(ContinuationChoice);
    }
  }
}
