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
    public string Id { get; protected set; }

    private object StateLock = new object();

    protected KnowledgeBase KnowledgeBase { get; set; }

    protected BotState State { get; set; }

    protected IBotEnvironment Environment { get; set; }

    protected WorkQueue<Request> WorkQueue { get; set; }

    protected bool IsRunning { get; set; }


    public Bot(KnowledgeBase kb)
    {
      Condition.Requires(kb, "kb").IsNotNull();

      Id = Guid.NewGuid().ToString();
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

        BotRepository.Add(this);
        Environment = env;
        WorkQueue = new WorkQueue<Request>();

        Thread botThread = new Thread(ThreadMain);
        botThread.IsBackground = true;
        botThread.Start();

        InitializeScheduler();

        BotHandle bh = new BotHandle(this, WorkQueue, botThread);

        // Give the bot a chance to emit a startup message
        bh.Invoke(new Request { Input = null });

        return bh;
      }
    }


    private void InitializeScheduler()
    {
      foreach (Domain d in KnowledgeBase.GetDomains())
      {
        d.RegisterScheduledJobs(StdSchedulerFactory.GetDefaultScheduler(), Id);
      }
    }


    private void ThreadMain()
    {
      bool shutdown = false;

      while (!shutdown)
      {
        try
        {
          Request request = WorkQueue.Dequeue(TimeSpan.FromDays(1));
          Invoke(request, callbackToEnvironment: true);
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
    }


    internal void Shutdown()
    {
      BotRepository.Remove(Id);
    }


    /// <summary>
    /// Invoke bot directly synchronously. This is the same method that gets called async by the background thread.
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Response Invoke(Request req, bool executeScheduledRules = false, bool callbackToEnvironment = false)
    {
      lock (StateLock)
      {
        List<string> output = new List<string>();

        if (req.Input != null)
        {
          ZTokenizer tokenizer = new ZTokenizer();
          ZStatementSequence statements = tokenizer.Tokenize(req.Input);

          // Always evaluate at least one empty statement in order to invoke triggers without regex
          if (statements.Statements.Count == 0)
            statements.Statements.Add(new ZTokenSequence());

          foreach (ZTokenSequence input in statements.Statements)
          {
            KnowledgeBase.ExpandTokens(input);
            EvaluationContext context = new EvaluationContext(State, input, req.RuleId, executeScheduledRules);
            ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

            if (reactions.Count > 0)
              foreach (Reaction r in reactions)
                output.Add(r.GenerateResponse(input));
            else
              output.Add("???");
          }
        }
        else
        {
          EvaluationContext context = new EvaluationContext(State, null, req.RuleId, executeScheduledRules);
          ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

          if (reactions.Count > 0)
            foreach (Reaction r in reactions)
              output.Add(r.GenerateResponse(new ZTokenSequence()));
        }

        if (output.Count > 0)
        {
          State["state.conversation.entries.Count"] = (double)State["state.conversation.entries.Count"] + 1;

          Response response = new Response
          {
            Output = output.ToArray()
          };

          if (callbackToEnvironment)
            Environment.HandleResponse(response);

          return response;
        }
        else
        {
          return new Response { Output = output.ToArray() };
        }
      }
    }
  }
}
