using System;
using System.Collections.Generic;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Knowledge
{
  public class ScheduledAction : Executable
  {
    public string Id { get; protected set; }

    public string CronExpr { get; protected set; }

    public Expression Condition { get; protected set; }


    public ScheduledAction(KnowledgeBase kb, string cronExpr, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, statements)
    {
      Id = Guid.NewGuid().ToString();
      CronExpr = cronExpr;
      RegisterModifiers(modifiers);
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


    public override void SetupComplete()
    {
    }
  }
}
