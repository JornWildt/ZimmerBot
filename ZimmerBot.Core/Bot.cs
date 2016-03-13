using System;
using System.Linq;
using System.Threading;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Language;
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

    protected bool IsRunning { get; set; }


    public Bot(KnowledgeBase kb)
    {
      Condition.Requires(kb, "kb").IsNotNull();

      KnowledgeBase = kb;
      State = new BotState();

      State["dialogue.responseCount"] = 0; // FIXME: use string constants
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

        BotHandle bh = new BotHandle(WorkQueue, botThread);

        // Give the bot a chance to emit a startup message
        bh.Invoke(new Request { Input = "" });

        return bh;
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
          Response response = Invoke(request);
          Environment.HandleResponse(response);
        }
        catch (ThreadAbortException)
        {
          shutdown = true;
        }
        catch (Exception)
        {
          // TODO: log this
        }
      }
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
        ZTokenString input = tokenizer.Tokenize(req.Input);

        KnowledgeBase.ExpandTokens(input);
        EvaluationContext context = new EvaluationContext(State, input);
        ReactionSet reactions = KnowledgeBase.FindMatchingReactions(context);

        string[] output;

        if (reactions.Count > 0)
          output = reactions.Select(r => r.GenerateResponse(input)).ToArray();
        else
          output = new string[] { "???" };

        State["dialogue.responseCount"] = (int)State["dialogue.responseCount"] + 1;

        return new Response
        {
          Output = output
        };
      }
    }
  }
}
