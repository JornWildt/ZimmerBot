using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class AnswerOutputStatement : OutputStatement
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public List<Rule> Rules { get; protected set; }


    public AnswerOutputStatement(KnowledgeBase kb, List<Rule> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      KnowledgeBase = kb;
      Rules = rules;
    }


    public override void Execute(OutputExecutionContect context)
    {
      // TO BE (RE)DONE
    }
  }
}
