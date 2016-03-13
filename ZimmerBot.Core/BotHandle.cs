using System.Threading;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core
{
  public class BotHandle
  {
    private Thread BotThread { get; set; }

    private WorkQueue<Request> WorkQueue { get; set; }


    public BotHandle(WorkQueue<Request> workQueue, Thread botThread)
    {
      Condition.Requires(workQueue, "workQueue").IsNotNull();
      Condition.Requires(botThread, "botThread").IsNotNull();

      WorkQueue = workQueue;
      BotThread = botThread;
    }


    public void Invoke(Request req)
    {
      WorkQueue.Enqueue(req);
    }
  }
}
