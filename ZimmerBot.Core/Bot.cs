using System;
using System.Collections.Generic;
using System.Threading;
using CuttingEdge.Conditions;
using log4net;
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

    protected IBotEnvironment Environment { get; set; }

    protected WorkQueue<Request> WorkQueue { get; set; }

    protected bool IsRunning { get; set; }


    public Bot(KnowledgeBase kb)
    {
      Condition.Requires(kb, "kb").IsNotNull();

      Id = Guid.NewGuid().ToString();
      KnowledgeBase = kb;
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

        return bh;
      }
    }


    private void InitializeScheduler()
    {
      KnowledgeBase.RegisterScheduledJobs(StdSchedulerFactory.GetDefaultScheduler(), Id);
    }


    private void ThreadMain()
    {
      bool shutdown = false;

      while (!shutdown)
      {
        try
        {
          Request request = WorkQueue.Dequeue(TimeSpan.FromDays(1));
          request.BotId = Id;

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
        Response response = BotUtility.Invoke(KnowledgeBase, req, executeScheduledRules);

        if (callbackToEnvironment)
          SendResponse(response);

        return response;
      }
    }


    public void SendResponse(Response response)
    {
      Environment.HandleResponse(response);
    }
  }
}
