using System.Collections.Generic;

namespace ZimmerBot.Core.Pipeline
{
  public interface IPipelineHandler<T>
  {
    void Handle(T item);
  }


  public class Pipeline<T>
  {
    protected List<IPipelineHandler<T>> Handlers { get; set; }


    public Pipeline()
    {
      Handlers = new List<IPipelineHandler<T>>();
    }


    public void AddHandler(IPipelineHandler<T> handler)
    {
      Handlers.Add(handler);
    }


    public void Invoke(T item)
    {
      foreach (var handler in Handlers)
      {
        handler.Handle(item);
      }
    }
  }
}
