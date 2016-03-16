using System;
using System.Threading;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core
{
  public class BotHandle
  {
    private Bot B { get; set; }

    private Thread BotThread { get; set; }

    private WorkQueue<Request> WorkQueue { get; set; }


    public BotHandle(Bot b, WorkQueue<Request> workQueue, Thread botThread)
    {
      Condition.Requires(b, "b").IsNotNull();
      Condition.Requires(workQueue, "workQueue").IsNotNull();
      Condition.Requires(botThread, "botThread").IsNotNull();

      B = b;
      WorkQueue = workQueue;
      BotThread = botThread;
    }


    /// <summary>
    /// Invoke bot asynchronously. It will callback with a response through the registered environment.
    /// </summary>
    /// <param name="req"></param>
    public void Invoke(Request req)
    {
      WorkQueue.Enqueue(req);
    }


    /// <summary>
    /// Stop background handling - abort bot thread.
    /// </summary>
    public void Shutdown()
    {
      BotThread.Abort();
      B.Shutdown();
    }
  }
}
