using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class AnswerStatement : Statement
  {
    public string Target { get; protected set; }

    public List<RuleBase> Rules { get; protected set; }


    public AnswerStatement(string target)
    {
      Condition.Requires(target, nameof(target)).IsNotNullOrEmpty();
      Target = target;
    }


    public AnswerStatement(List<RuleBase> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Rules = rules;
    }


    public override RepatableMode Repeatable
    {
      get { return RepatableMode.Undefined; }
    }


    public override void Initialize(StatementInitializationContext context)
    {
      if (Rules != null)
        foreach (RuleBase r in Rules)
          r.RegisterParentRule(context.ParentRule);
    }


    public override void Execute(StatementExecutionContect context)
    {
      if (Target != null)
      {
        Continuation c = new Continuation(Continuation.ContinuationEnum.Answer, Target);
        context.Continue(c);
      }
    }
  }
}

