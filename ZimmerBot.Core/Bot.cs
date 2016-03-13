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
    protected KnowledgeBase KnowledgeBase { get; set; }

    protected BotState State { get; set; }

    protected IBotEnvironment Environment { get; set; }

    protected WorkQueue<Request> WorkQueue { get; set; }


    public Bot(KnowledgeBase kb)
    {
      Condition.Requires(kb, "kb").IsNotNull();

      KnowledgeBase = kb;
      State = new BotState();

      State["dialogue.responseCount"] = 0; // FIXME: use string constants
    }



    public BotHandle Run(IBotEnvironment env)
    {
      Condition.Requires(env, "env").IsNotNull();

      Environment = env;
      WorkQueue = new WorkQueue<Request>();

      Thread botThread = new Thread(BackgroundWork);
      botThread.IsBackground = true;
      botThread.Start();

      BotHandle bh = new BotHandle(WorkQueue, botThread);
      bh.Invoke(new Request { Input = "" });

      return bh;
    }


    private void BackgroundWork()
    {
      bool shutdown = false;

      while (!shutdown)
      {
        Request request = WorkQueue.Dequeue(TimeSpan.FromDays(1));
        Response response = Invoke(request);
        Environment.HandleResponse(response);
      }
    }


    public Response Invoke(Request req)
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
