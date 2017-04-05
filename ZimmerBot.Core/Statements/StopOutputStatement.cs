using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Scheduler;

namespace ZimmerBot.Core.Statements
{
  public class StopOutputStatement : Statement
  {
    public override void Execute(StatementExecutionContect context)
    {
      ScheduleHelper.StopDelayedMessages(context);
    }

    public override void Initialize(StatementInitializationContext context)
    {
      // Do nothing
    }
  }
}
