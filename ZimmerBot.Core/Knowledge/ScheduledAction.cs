using System;
using System.Collections.Generic;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Knowledge
{
  public class ScheduledAction : Executable
  {
    public string Id { get; protected set; }

    public string CronExpr { get; protected set; }

    public List<RuleModifier> Modifiers { get; protected set; }


    public ScheduledAction(KnowledgeBase kb, string cronExpr, List<RuleModifier> modifiers, List<Statement> statements)
      : base(kb, statements)
    {
      Id = Guid.NewGuid().ToString();
      CronExpr = cronExpr;
      Modifiers = modifiers;
    }


    public override void SetupComplete()
    {
    }
  }
}
