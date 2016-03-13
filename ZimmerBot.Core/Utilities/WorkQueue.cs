using System;
using System.Collections.Generic;
using System.Threading;


namespace ZimmerBot.Core.Utilities
{
  public class WorkQueue<T> where T : class
  {
    private object QueueLock = new object();

    private Queue<T> Queue = new Queue<T>();

    private ManualResetEvent NewItemAvailable = new ManualResetEvent(false);


    public void Enqueue(T item)
    {
      lock (QueueLock)
      {
        Queue.Enqueue(item);
        if (Queue.Count == 1)
          NewItemAvailable.Set();
      }
    }


    public T Dequeue(TimeSpan timeout)
    {
      bool gotItem = NewItemAvailable.WaitOne(timeout);
      if (!gotItem)
        return null;

      lock (QueueLock)
      {
        if (Queue.Count == 0)
          return null;

        T item = Queue.Dequeue();
        if (Queue.Count == 0)
          NewItemAvailable.Reset();

        return item;
      }
    }
  }
}
