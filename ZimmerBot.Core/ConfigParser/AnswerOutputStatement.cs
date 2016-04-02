using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class AnswerOutputStatement : OutputStatement
  {
    public KnowledgeBase KnowledgeBase { get; protected set; }

    public List<Func<KnowledgeBase, Rule>> RuleGenerators { get; protected set; }


    public AnswerOutputStatement(KnowledgeBase kb, List<Func<KnowledgeBase, Rule>> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      KnowledgeBase = kb;
      RuleGenerators = rules;
    }


    public override void Execute(OutputExecutionContect context)
    {
      foreach (var generator in RuleGenerators)
      {
        Rule r = generator(KnowledgeBase);
      }
    }
  }
}
