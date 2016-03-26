using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class AnswerOutputStatement : OutputStatement
  {
    public Domain Domain { get; protected set; }
    public List<Func<Domain, Rule>> RuleGenerators { get; protected set; }


    public AnswerOutputStatement(Domain domain, List<Func<Domain,Rule>> rules)
    {
      Condition.Requires(rules, nameof(rules)).IsNotNull();
      Condition.Requires(domain, nameof(domain)).IsNotNull();
      Domain = domain;
      RuleGenerators = rules;
    }


    public override void Execute(OutputExecutionContect context)
    {
      foreach (var generator in RuleGenerators)
      {
        Rule r = generator(Domain).AsAnswer();
      }
    }
  }
}
