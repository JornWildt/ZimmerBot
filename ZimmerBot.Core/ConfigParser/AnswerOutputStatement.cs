using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class AnswerOutputStatement : OutputStatement
  {
    public List<Rule> Rules { get; protected set; }


    public AnswerOutputStatement(List<Rule> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Rules = rules;
    }


    public override void Initialize(OutputInitializationContext context)
    {
      foreach (Rule r in Rules)
        r.RegisterParentRule(context.ParentRule);
    }


    public override void Execute(OutputExecutionContect context)
    {
      // Nothing here
    }
  }
}

