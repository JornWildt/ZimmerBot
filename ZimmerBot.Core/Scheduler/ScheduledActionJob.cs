﻿using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.Scheduler
{
  public class ScheduledActionJob : IJob
  {
    private static ILog Logger = LogManager.GetLogger(typeof(ScheduledActionJob));

    private static object ActionLocker = new object();


    public void Execute(IJobExecutionContext jobContext)
    {
      Logger.Debug("Execute scheduled action.");

      lock (ActionLocker)
      {
        var botId = jobContext.JobDetail.JobDataMap.GetString("BotId");
        var bot = BotRepository.Get(botId);

        var actionId = jobContext.JobDetail.JobDataMap.GetString("ActionId");
        var action = bot.KnowledgeBase.GetScheduledAction(actionId);

        foreach (var session in SessionManager.GetSessions())
        {
          Request request = new Request(session.SessionId, session.Store[SessionKeys.UserId])
          {
            BotId = botId,
          };
          RequestContext requestContext = BotUtility.BuildRequestContext(bot.KnowledgeBase, request);

          bool canExecute = true;
          if (action.Condition != null)
          {
            ExpressionEvaluationContext eec = new ExpressionEvaluationContext(requestContext.Session, requestContext.State.State);
            object value = action.Condition.Evaluate(eec);
            if (!Expression.TryConvertToBool(value, out canExecute))
              throw new InvalidCastException($"Could not convert value '{value}' to bool in condition.");
          }

          if (canExecute)
          {
            ZTokenSequence input = new ZTokenSequence();
            InputRequestContext inputContext = new InputRequestContext(requestContext, request, input);
            ResponseGenerationContext responseContext = new ResponseGenerationContext(inputContext, new WordRegex.MatchResult(0.0));

            List<string> output = action.Invoke(responseContext, null);

            if (output.Count > 0)
            {
              string message = output[0];
              Response response = new Response(new string[] { message }, null, session);
              bot.SendResponse(response);
            }
          }
        }
      }
    }
  }
}
