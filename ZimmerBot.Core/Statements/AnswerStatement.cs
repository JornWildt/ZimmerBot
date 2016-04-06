using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Statements
{
  public class AnswerStatement : Statement
  {
    public List<Rule> Rules { get; protected set; }


    public AnswerStatement(List<Rule> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Rules = rules;
    }


    public override void Initialize(StatementInitializationContext context)
    {
      foreach (Rule r in Rules)
        r.RegisterParentRule(context.ParentRule);
    }


    public override void Execute(StatementExecutionContect context)
    {
      // Nothing here
    }
  }
}

