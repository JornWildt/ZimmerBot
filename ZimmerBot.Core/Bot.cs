using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CuttingEdge.Conditions;
using Quartz;
using Quartz.Impl;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core
{
  public class Bot
  {
    private object StateLock = new object();

    protected KnowledgeBase KnowledgeBase { get; set; }

    protected BotState State { get; set; }

    protected IBotEnvironment Environment { get; set; }

    protected WorkQueue<Request> WorkQueue { get; set; }

    protected IScheduler Scheduler { get; set; }

    protected bool IsRunning { get; set; }


    public Bot(KnowledgeBase kb)
    {
      Condition.Requires(kb, "kb").IsNotNull();

      KnowledgeBase = kb;
      State = new BotState();

      State["state.conversation.entries.Count"] = 0d; // FIXME: use string constants
    }


    /// <summary>
    /// Start asynchronous bot interaction.
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    public BotHandle Run(IBotEnvironment env)
    {
      Condition.Requires(env, "env").IsNotNull();

      lock (StateLock)
      {
        if (IsRunning)
          throw new InvalidOperationException("Cannot run bot asynchronously twice.");
        IsRunning = true;

        Environment = env;
        WorkQueue = new WorkQueue<Request>();

        Thread botThread = new Thread(ThreadMain);
        botThread.IsBackground = true;
        botThread.Start();

        InitializeScheduler();

        BotHandle bh = new BotHandle(this, WorkQueue, botThread);

        // Give the bot a chance to emit a startup message
        bh.Invoke(new Request { Input = "" });

        return bh;
      }
    }


    private void InitializeScheduler()
    {
      Scheduler = StdSchedulerFactory.GetDefaultScheduler();

      foreach (Domain d in KnowledgeBase.GetDomains())
      {
        d.RegisterScheduledJobs(Scheduler);
      }

      //IJobDetail job = JobBuilder.Create<SchedulerJob>()
      //    .WithIdentity("job2", "group1")
      //    .Build();

      //ITrigger trigger = TriggerBuilder.Create()
      //    .WithIdentity("trigger2", "group1")
      //    .StartNow()
      //    .WithSimpleSchedule(x => x
      //        .WithIntervalInSeconds(10)
      //        .RepeatForever())
      //    .Build();

      //// Tell quartz to schedule the job using our trigger
      //Scheduler.ScheduleJob(job, trigger);

      Scheduler.Start();

    }


    private void ThreadMain()
    {
      bool shutdown = false;

      while (!shutdown)
      {
        try
        {
          Request request = WorkQueue.Dequeue(TimeSpan.FromDays(1));
          Response response = Invoke(request);
          Environment.HandleResponse(response);
        }
        catch (ThreadAbortException)
        {
          shutdown = true;
        }
        catch (Exception ex)
        {
          Environment.Log(LogLevel.Error, "Error in bot background thread", ex);
        }
      }

      Scheduler.Shutdown();
    }


    internal void Shutdown()
    {
      Scheduler.Shutdown();
    }


    /// <summary>
    /// Invoke bot directly synchronously. This is the same method that gets called async by the background thread.
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Response Invoke(Request req)
    {
      lock (StateLock)
      {
        ZTokenizer tokenizer = new ZTokenizer();
        ZStatementSequence statements = tokenizer.Tokenize(req.Input);
        List<string> output = new List<string>();

        // Always evaluate at least one empty statement in order to invoke triggers without regex
        if (statements.Statements.Count == 0)
          statements.Statements.Add(new ZTokenSequence());

        foreach (ZTokenSequence input in statements.Statements)
        {
          KnowledgeBase.ExpandTokens(input);
          EvaluationContext context = new EvaluationContext(State, input);
          ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

          if (reactions.Count > 0)
            foreach (Reaction r in reactions)
              output.Add(r.GenerateResponse(input));
          else
            output.Add("???");

          State["state.conversation.entries.Count"] = (double)State["state.conversation.entries.Count"] + 1;
        }

        return new Response
        {
          Output = output.ToArray()
        };
      }
    }


    class SchedulerJob : IJob
    {
      public void Execute(IJobExecutionContext context)
      {
        Console.WriteLine("SCHEDULE");
      }
    }
  }
}
